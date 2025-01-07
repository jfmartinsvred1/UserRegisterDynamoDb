using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UserRegisterDynamo.Models;
using UserRegisterDynamo.Services;

namespace UserRegisterDynamo
{
    public class SecretSettings
    {
        public string Secret { get; set; }
    }
    public class TokenService
    {
        private string SecretKey { get; set; }
        public TokenService(IOptions<SecretSettings> secretSettings)
        {
            var settings = secretSettings.Value;
            this.SecretKey=settings.Secret;
        }
        public string GenerateToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(this.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials= new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Subject= new System.Security.Claims.ClaimsIdentity( new Claim[]
                {
                    new Claim(ClaimTypes.Name, username.ToString()),
                }
                )

            };

            var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
            return token;
        }
    }
}