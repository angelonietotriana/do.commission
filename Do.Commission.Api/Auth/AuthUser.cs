namespace Do.Commission_.Auth
{
    public sealed class AuthUser
    {
        public string UserName { get; init; } = string.Empty;
        public string PasswordHash { get; init; } = string.Empty;
        public string Role { get; init; } = AuthRoles.Usuario;
    }
}
