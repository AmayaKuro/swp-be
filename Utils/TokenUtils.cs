using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;
using swp_be.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace swp_be.Utils
{
    public enum TokenType
    {
        ACCESS_TOKEN = 0,
        REFRESH_TOKEN = 1
    }

    public class TokenUtils
    {
        private string Secret = Environment.GetEnvironmentVariable("Swp_Secret_key") ?? "YSBsb25nIEFzc3MgIHN0cmluZyB0aGF0J3MgbGFzc3QgNjQgY2hhcmFjdGVyPz8/";

        public string Sign(User user, TokenType type)
        {
            DateTime now = DateTime.UtcNow;
            var claims = new List<Claim>()
            {
                new Claim("userID", user.UserID.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
            };

            if (type == TokenType.ACCESS_TOKEN)
            {
                claims.Add(new Claim(ClaimTypes.Role, user.Role.ToString()));
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),

                Expires =  now.AddDays(7),

                Issuer = Configuration.GetConfiguration()["BEUrl"],
                Audience = Configuration.GetConfiguration()["FEUrl"],

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret)),
                    SecurityAlgorithms.HmacSha256Signature),
            };

            

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;
        }
    }
}
