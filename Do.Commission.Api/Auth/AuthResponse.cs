namespace Do.Commission_.Auth
{
    public sealed class AuthResponse
    {
        public string Token { get; init; } = string.Empty;
        public string UserName { get; init; } = string.Empty;
        public string Role { get; init; } = string.Empty;
        public DateTime ExpiresAtUtc { get; init; }
    }
}
