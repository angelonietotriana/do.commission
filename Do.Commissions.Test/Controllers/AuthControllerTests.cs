using Do.Commission_.Auth;
using Do.Commission_.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Do.Commissions.Test.Controllers;

public class AuthControllerTests
{
    [Fact]
    public void Register_WhenUserNameIsMissing_ReturnsBadRequest()
    {
        var controller = CreateController();

        var result = controller.Register(new RegisterRequest
        {
            UserName = string.Empty,
            Password = "123456",
            Role = AuthRoles.Usuario
        });

        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Usuario y contraseña son requeridos.", badRequest.Value);
    }

    [Fact]
    public void Register_WhenPasswordIsMissing_ReturnsBadRequest()
    {
        var controller = CreateController();

        var result = controller.Register(new RegisterRequest
        {
            UserName = "usuario1",
            Password = string.Empty,
            Role = AuthRoles.Usuario
        });

        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Usuario y contraseña son requeridos.", badRequest.Value);
    }

    [Fact]
    public void Register_WhenRoleIsInvalid_ReturnsBadRequest()
    {
        var controller = CreateController();

        var result = controller.Register(new RegisterRequest
        {
            UserName = "usuario1",
            Password = "123456",
            Role = "Invitado"
        });

        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("El rol debe ser Administrador o Usuario.", badRequest.Value);
    }

    [Fact]
    public void Register_WhenUserAlreadyExists_ReturnsConflict()
    {
        var controller = CreateController();
        controller.Register(new RegisterRequest
        {
            UserName = "usuario1",
            Password = "123456",
            Role = AuthRoles.Usuario
        });

        var result = controller.Register(new RegisterRequest
        {
            UserName = "usuario1",
            Password = "654321",
            Role = AuthRoles.Administrador
        });

        var conflict = Assert.IsType<ConflictObjectResult>(result.Result);
        Assert.Equal("El usuario ya existe.", conflict.Value);
    }

    [Fact]
    public void Register_WhenRequestIsValid_ReturnsOkWithToken()
    {
        var controller = CreateController();

        var result = controller.Register(new RegisterRequest
        {
            UserName = "admin1",
            Password = "123456",
            Role = AuthRoles.Administrador
        });

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<AuthResponse>(okResult.Value);

        Assert.Equal("admin1", response.UserName);
        Assert.Equal(AuthRoles.Administrador, response.Role);
        Assert.False(string.IsNullOrWhiteSpace(response.Token));
        Assert.InRange(response.ExpiresAtUtc, DateTime.UtcNow.AddHours(5).AddMinutes(-1), DateTime.UtcNow.AddHours(5).AddMinutes(1));
    }

    [Fact]
    public void Login_WhenUserNameIsMissing_ReturnsBadRequest()
    {
        var controller = CreateController();

        var result = controller.Login(new LoginRequest
        {
            UserName = string.Empty,
            Password = "123456"
        });

        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Usuario y contraseña son requeridos.", badRequest.Value);
    }

    [Fact]
    public void Login_WhenPasswordIsMissing_ReturnsBadRequest()
    {
        var controller = CreateController();

        var result = controller.Login(new LoginRequest
        {
            UserName = "usuario1",
            Password = string.Empty
        });

        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Usuario y contraseña son requeridos.", badRequest.Value);
    }

    [Fact]
    public void Login_WhenCredentialsAreInvalid_ReturnsUnauthorized()
    {
        var controller = CreateController();

        var result = controller.Login(new LoginRequest
        {
            UserName = "usuario1",
            Password = "incorrecta"
        });

        var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result.Result);
        Assert.Equal("Credenciales inválidas.", unauthorized.Value);
    }

    [Fact]
    public void Login_WhenCredentialsAreValid_ReturnsOkWithToken()
    {
        var controller = CreateController();
        controller.Register(new RegisterRequest
        {
            UserName = "usuario1",
            Password = "123456",
            Role = AuthRoles.Usuario
        });

        var result = controller.Login(new LoginRequest
        {
            UserName = "usuario1",
            Password = "123456"
        });

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<AuthResponse>(okResult.Value);

        Assert.Equal("usuario1", response.UserName);
        Assert.Equal(AuthRoles.Usuario, response.Role);
        Assert.False(string.IsNullOrWhiteSpace(response.Token));
    }

    [Fact]
    public void Login_WhenPasswordIsIncorrectForExistingUser_ReturnsUnauthorized()
    {
        var controller = CreateController();
        controller.Register(new RegisterRequest
        {
            UserName = "usuario1",
            Password = "123456",
            Role = AuthRoles.Usuario
        });

        var result = controller.Login(new LoginRequest
        {
            UserName = "usuario1",
            Password = "654321"
        });

        var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result.Result);
        Assert.Equal("Credenciales inválidas.", unauthorized.Value);
    }

    private static AuthController CreateController()
    {
        var userStore = new InMemoryUserStore();
        var tokenService = new JwtTokenService(CreateConfiguration());
        return new AuthController(userStore, tokenService);
    }

    private static IConfiguration CreateConfiguration()
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Issuer"] = "Do.Commission.Api.Tests",
                ["Jwt:Audience"] = "Do.Commission.Client.Tests",
                ["Jwt:SecretKey"] = "DoCommissionJwtSecretKey2026_SuperSegura_12345",
                ["Jwt:ExpirationHours"] = "5"
            })
            .Build();
    }
}
