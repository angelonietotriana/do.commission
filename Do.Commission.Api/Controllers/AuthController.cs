using Do.Commission_.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Do.Commission_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(InMemoryUserStore userStore, JwtTokenService jwtTokenService) : ControllerBase
    {
        private const string RegisterRoute = "register";
        private const string LoginRoute = "login";
        private const string RequiredCredentialsMessage = "Usuario y contraseña son requeridos.";
        private const string InvalidRoleMessage = "El rol debe ser Administrador o Usuario.";
        private const string UserAlreadyExistsMessage = "El usuario ya existe.";
        private const string RegisterFailedMessage = "No fue posible registrar el usuario.";
        private const string InvalidCredentialsMessage = "Credenciales inválidas.";

        private readonly InMemoryUserStore _userStore = userStore;
        private readonly JwtTokenService _jwtTokenService = jwtTokenService;

        /// <summary>
        /// Registra un usuario nuevo en memoria y devuelve un token JWT si el proceso se completa correctamente.
        /// </summary>
        /// <param name="request">Datos del usuario, contraseña y rol que se desean registrar.</param>
        /// <returns>
        /// Un resultado <see cref="BadRequestObjectResult"/> si faltan datos o el rol no es válido,
        /// <see cref="ConflictObjectResult"/> si el usuario ya existe,
        /// o <see cref="OkObjectResult"/> con el token generado.
        /// </returns>
        [HttpPost(RegisterRoute)]
        public ActionResult<AuthResponse> Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(RequiredCredentialsMessage);

            if (!InMemoryUserStore.IsValidRole(request.Role))
                return BadRequest(InvalidRoleMessage);

            if (_userStore.Exists(request.UserName))
                return Conflict(UserAlreadyExistsMessage);

            if (!_userStore.TryRegister(request, out var user) || user is null)
                return BadRequest(RegisterFailedMessage);

            return Ok(_jwtTokenService.GenerateToken(user));
        }

        /// <summary>
        /// Valida las credenciales de un usuario registrado y devuelve un token JWT cuando son correctas.
        /// </summary>
        /// <param name="request">Nombre de usuario y contraseña que se usarán para autenticarse.</param>
        /// <returns>
        /// Un resultado <see cref="BadRequestObjectResult"/> si faltan datos,
        /// <see cref="UnauthorizedObjectResult"/> si las credenciales no son válidas,
        /// o <see cref="OkObjectResult"/> con el token generado.
        /// </returns>
        [HttpPost(LoginRoute)]
        public ActionResult<AuthResponse> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(RequiredCredentialsMessage);

            var user = _userStore.ValidateCredentials(request);
            if (user is null)
                return Unauthorized(InvalidCredentialsMessage);

            return Ok(_jwtTokenService.GenerateToken(user));
        }
    }
}
