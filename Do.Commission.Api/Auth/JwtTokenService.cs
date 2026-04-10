using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Do.Commission_.Auth
{
    public sealed class JwtTokenService(IConfiguration configuration)
    {
        private static readonly string JwtIssuerKey = "Jwt:Issuer";
        private static readonly string JwtAudienceKey = "Jwt:Audience";
        private static readonly string JwtSecretKey = "Jwt:SecretKey";
        private static readonly string JwtExpirationHoursKey = "Jwt:ExpirationHours";
        private static readonly string MissingSecretKeyMessage = "La llave JWT no está configurada.";

        private readonly IConfiguration _configuration = configuration;

        public AuthResponse GenerateToken(AuthUser user)
        {
            var expiresAtUtc = DateTime.UtcNow.AddHours(GetExpirationHours());
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.UserName),
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Role, user.Role),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetSecretKey())),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration[JwtIssuerKey],
                audience: _configuration[JwtAudienceKey],
                claims: claims,
                expires: expiresAtUtc,
                signingCredentials: credentials);

            return new AuthResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                UserName = user.UserName,
                Role = user.Role,
                ExpiresAtUtc = expiresAtUtc
            };
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            try
            {
                return new JwtSecurityTokenHandler().ValidateToken(token, CreateValidationParameters(), out _);
            }
            catch
            {
                return null;
            }
        }

        private TokenValidationParameters CreateValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _configuration[JwtIssuerKey],
                ValidateAudience = true,
                ValidAudience = _configuration[JwtAudienceKey],
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetSecretKey())),
                ClockSkew = TimeSpan.Zero
            };
        }

        private string GetSecretKey()
        {
            return _configuration[JwtSecretKey]
                ?? throw new InvalidOperationException(MissingSecretKeyMessage);
        }

        private int GetExpirationHours()
        {
            return _configuration.GetValue<int?>(JwtExpirationHoursKey) ?? 5;
        }
    }
}
