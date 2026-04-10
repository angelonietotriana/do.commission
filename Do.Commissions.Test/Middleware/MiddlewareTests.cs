using Do.Commission_.Auth;
using EmployeesMiddleware = Do.Commission_.Middleware.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IO;
using Xunit;

namespace Do.Commissions.Test.Middleware;

public class MiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_WhenTokenIsMissing_ReturnsUnauthorized()
    {
        var nextCalled = false;
        var middleware = CreateMiddleware(_ =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        });
        var context = CreateContext(HttpMethods.Get, "/api/employees");

        await middleware.InvokeAsync(context, CreateJwtTokenService());

        Assert.False(nextCalled);
        Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
        Assert.Equal("Token JWT requerido.", ReadResponseBody(context));
    }

    [Fact]
    public async Task InvokeAsync_WhenTokenIsInvalid_ReturnsUnauthorized()
    {
        var nextCalled = false;
        var middleware = CreateMiddleware(_ =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        });
        var context = CreateContext(HttpMethods.Get, "/api/employees", "token-invalido");

        await middleware.InvokeAsync(context, CreateJwtTokenService());

        Assert.False(nextCalled);
        Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
        Assert.Equal("Token JWT inválido o expirado.", ReadResponseBody(context));
    }

    [Fact]
    public async Task InvokeAsync_WhenRoleIsUsuarioAndMethodIsPost_ReturnsForbidden()
    {
        var nextCalled = false;
        var middleware = CreateMiddleware(_ =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        });
        var token = CreateToken(AuthRoles.Usuario, "user1");
        var context = CreateContext(HttpMethods.Post, "/api/employees", token);

        await middleware.InvokeAsync(context, CreateJwtTokenService());

        Assert.False(nextCalled);
        Assert.Equal(StatusCodes.Status403Forbidden, context.Response.StatusCode);
        Assert.Equal("El rol del usuario no tiene permisos para realizar esta acción.", ReadResponseBody(context));
    }

    [Fact]
    public async Task InvokeAsync_WhenRoleIsUsuarioAndMethodIsGet_AllowsRequest()
    {
        var nextCalled = false;
        var middleware = CreateMiddleware(context =>
        {
            nextCalled = true;
            context.Response.StatusCode = StatusCodes.Status204NoContent;
            return Task.CompletedTask;
        });
        var token = CreateToken(AuthRoles.Usuario, "user1");
        var context = CreateContext(HttpMethods.Get, "/api/employees", token);

        await middleware.InvokeAsync(context, CreateJwtTokenService());

        Assert.True(nextCalled);
        Assert.Equal(StatusCodes.Status204NoContent, context.Response.StatusCode);
        Assert.True(context.User.Identity?.IsAuthenticated ?? false);
        Assert.True(context.User.IsInRole(AuthRoles.Usuario));
    }

    [Fact]
    public async Task InvokeAsync_WhenRoleIsAdministradorAndMethodIsPost_AllowsRequest()
    {
        var nextCalled = false;
        var middleware = CreateMiddleware(context =>
        {
            nextCalled = true;
            context.Response.StatusCode = StatusCodes.Status204NoContent;
            return Task.CompletedTask;
        });
        var token = CreateToken(AuthRoles.Administrador, "admin1");
        var context = CreateContext(HttpMethods.Post, "/api/employees", token);

        await middleware.InvokeAsync(context, CreateJwtTokenService());

        Assert.True(nextCalled);
        Assert.Equal(StatusCodes.Status204NoContent, context.Response.StatusCode);
        Assert.True(context.User.IsInRole(AuthRoles.Administrador));
    }

    [Fact]
    public async Task InvokeAsync_WhenEmployeeIdIsInvalid_ReturnsBadRequest()
    {
        var nextCalled = false;
        var middleware = CreateMiddleware(_ =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        });
        var token = CreateToken(AuthRoles.Administrador, "admin1");
        var context = CreateContext(HttpMethods.Get, "/api/employees/0", token);

        await middleware.InvokeAsync(context, CreateJwtTokenService());

        Assert.False(nextCalled);
        Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);
        Assert.Equal("El parámetro debe ser un documento del empleado válido.", ReadResponseBody(context));
    }

    private static EmployeesMiddleware CreateMiddleware(RequestDelegate next)
    {
        return new EmployeesMiddleware(next);
    }

    private static DefaultHttpContext CreateContext(string method, string path, string? token = null)
    {
        var context = new DefaultHttpContext();
        context.Request.Method = method;
        context.Request.Path = path;
        context.Response.Body = new MemoryStream();

        if (!string.IsNullOrWhiteSpace(token))
            context.Request.Headers.Authorization = $"Bearer {token}";

        return context;
    }

    private static string CreateToken(string role, string userName)
    {
        var tokenService = CreateJwtTokenService();
        var response = tokenService.GenerateToken(new AuthUser
        {
            UserName = userName,
            Role = role,
            PasswordHash = string.Empty
        });

        return response.Token;
    }

    private static JwtTokenService CreateJwtTokenService()
    {
        return new JwtTokenService(new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Issuer"] = "Do.Commission.Api.Tests",
                ["Jwt:Audience"] = "Do.Commission.Client.Tests",
                ["Jwt:SecretKey"] = "DoCommissionJwtSecretKey2026_SuperSegura_12345",
                ["Jwt:ExpirationHours"] = "5"
            })
            .Build());
    }

    private static string ReadResponseBody(HttpContext context)
    {
        context.Response.Body.Position = 0;
        using var reader = new StreamReader(context.Response.Body, leaveOpen: true);
        return reader.ReadToEnd();
    }
}
