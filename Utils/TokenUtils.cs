using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;
using swp_be.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace swp_be.Utils
{
    public enum TokenType
    {
        ACCESS_TOKEN,
        REFRESH_TOKEN
    }

    public class TokenUtils
    {
        private string Secret = Environment.GetEnvironmentVariable("Swp_Secret_key", System.EnvironmentVariableTarget.User) ?? "YSBsb25nIEFzc3MgIHN0cmluZyB0aGF0J3MgbGFzc3QgNjQgY2hhcmFjdGVyPz8/";

        public string Sign(User user, TokenType type)
        {
            DateTime now = DateTime.UtcNow;
            var payload = new
            {
                userID = user.UserID,
                username = user.Username,
                role = user.Role,
                name = user.Name,
            };

            var symmetricKey = Convert.FromBase64String(Secret);
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("user", payload.ToString())
                }),

                Expires = type == TokenType.REFRESH_TOKEN ? now.AddDays(7) : now.AddMinutes(30),

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(symmetricKey),
                    SecurityAlgorithms.HmacSha256Signature),

            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;
        }
    }
}
