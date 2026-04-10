using Microsoft.AspNetCore.Identity;
using System.Collections.Concurrent;

namespace Do.Commission_.Auth
{
    public sealed class InMemoryUserStore
    {
        private readonly ConcurrentDictionary<string, AuthUser> _users = new(System.StringComparer.OrdinalIgnoreCase);
        private readonly PasswordHasher<AuthUser> _passwordHasher = new();

        public bool TryRegister(RegisterRequest request, out AuthUser? user)
        {
            user = null;

            var normalizedRole = NormalizeRole(request.Role);
            if (normalizedRole is null)
                return false;

            var authUser = new AuthUser
            {
                UserName = request.UserName.Trim(),
                Role = normalizedRole
            };

            var hashedUser = new AuthUser
            {
                UserName = authUser.UserName,
                Role = authUser.Role,
                PasswordHash = _passwordHasher.HashPassword(authUser, request.Password)
            };

            if (!_users.TryAdd(hashedUser.UserName, hashedUser))
                return false;

            user = hashedUser;
            return true;
        }

        public AuthUser? ValidateCredentials(LoginRequest request)
        {
            if (!_users.TryGetValue(request.UserName.Trim(), out var user))
                return null;

            var verification = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            return verification == PasswordVerificationResult.Failed ? null : user;
        }

        public bool Exists(string userName)
        {
            return _users.ContainsKey(userName.Trim());
        }

        public static bool IsValidRole(string role)
        {
            return NormalizeRole(role) is not null;
        }

        private static string? NormalizeRole(string role)
        {
            if (string.Equals(role, AuthRoles.Administrador, System.StringComparison.OrdinalIgnoreCase))
                return AuthRoles.Administrador;

            if (string.Equals(role, AuthRoles.Usuario, System.StringComparison.OrdinalIgnoreCase))
                return AuthRoles.Usuario;

            return null;
        }
    }
}
