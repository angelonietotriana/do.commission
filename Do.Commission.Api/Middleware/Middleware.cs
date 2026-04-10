using Do.Commission_.Auth;
using System.Security.Claims;

namespace Do.Commission_.Middleware
{
    public class Middleware(RequestDelegate next)
    {
        private const string BasePath = "/api/employees";
        private const string ExceptionMessage = "El parámetro debe ser un documento del empleado válido.";
        private const string BearerPrefix = "Bearer ";
        private const string EmptyToken = "";
        private const string MissingTokenMessage = "Token JWT requerido.";
        private const string InvalidTokenMessage = "Token JWT inválido o expirado.";
        private const string ForbiddenMessage = "El rol del usuario no tiene permisos para realizar esta acción.";

        private readonly RequestDelegate _next = next;

        /// <summary>
        /// Valida el token JWT y el rol del usuario antes de permitir el acceso a los endpoints de empleados.
        /// También valida que el identificador enviado en los GET por id sea un número válido mayor que cero.
        /// </summary>
        /// <param name="context">Contexto HTTP de la solicitud actual.</param>
        /// <param name="jwtTokenService">Servicio encargado de validar el token recibido.</param>
        /// <returns>Una tarea asincrónica que finaliza cuando la solicitud es procesada.</returns>
        public async Task InvokeAsync(HttpContext context, JwtTokenService jwtTokenService)
        {
            if (context.Request.Path.StartsWithSegments(BasePath))
            {
                var authorizationHeader = context.Request.Headers.Authorization.ToString();
                var token = authorizationHeader.StartsWith(BearerPrefix, System.StringComparison.OrdinalIgnoreCase)
                    ? authorizationHeader[BearerPrefix.Length..].Trim()
                    : EmptyToken;

                if (string.IsNullOrWhiteSpace(token))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync(MissingTokenMessage);
                    return;
                }

                var principal = jwtTokenService.ValidateToken(token);
                if (principal is null)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync(InvalidTokenMessage);
                    return;
                }

                context.User = principal;

                if (!HasRequiredRole(context.Request.Method, principal))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync(ForbiddenMessage);
                    return;
                }
            }

            if (context.Request.Method == HttpMethods.Get &&
                context.Request.Path.StartsWithSegments(BasePath, out var remaining) &&
                remaining.HasValue && remaining.Value.Trim('/').All(char.IsDigit))
            {
                var idSegment = remaining.Value.Trim('/');
                if (!int.TryParse(idSegment, out var id) || id <= decimal.Zero)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync(ExceptionMessage);
                    return;
                }
            }

            await _next(context);
        }

        private static bool HasRequiredRole(string method, ClaimsPrincipal principal)
        {
            if (method == HttpMethods.Get)
                return principal.IsInRole(AuthRoles.Administrador) || principal.IsInRole(AuthRoles.Usuario);

            return principal.IsInRole(AuthRoles.Administrador);
        }
    }
}
