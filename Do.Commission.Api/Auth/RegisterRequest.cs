namespace Do.Commission_.Auth
{
    public sealed class RegisterRequest
    {
        public string UserName { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public string Role { get; init; } = AuthRoles.Usuario;
    }
}
