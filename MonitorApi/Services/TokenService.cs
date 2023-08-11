using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MonitorApi.Models.Setting;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MonitorApi.Services
{
    public class TokenService
    {
        private const int ExpirationMinutes = 30;
        private readonly IOptions<TokensSettings> _tokensSettings;
        public TokenService(IOptions<TokensSettings> tokensSettings) => _tokensSettings = tokensSettings;
        public string CreateToken(IdentityUser user)
        {
            try
            {
                var expiration = DateTime.UtcNow.AddMinutes(ExpirationMinutes);
                var token = CreateJwtToken(
                    CreateClaims(user),
                    CreateSigningCredentials(),
                    expiration
                    );
                var tokenHandler = new JwtSecurityTokenHandler();
                var ss = tokenHandler.WriteToken(token);
                return ss;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials,
            DateTime expiration) => new JwtSecurityToken(
                _tokensSettings.Value.Issuer,
                _tokensSettings.Value.Issuer,
                claims,
                expires: expiration,
                signingCredentials: credentials
                );

        private List<Claim> CreateClaims(IdentityUser user)
        {
            try
            {
                var claims = new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.Sub, "TokenForTheApiWithAuth"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, "admin"), 

                };
                return claims;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        private SigningCredentials CreateSigningCredentials()
        {
            return new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokensSettings.Value.Key)),
                SecurityAlgorithms.HmacSha256);
        }
    }
}
