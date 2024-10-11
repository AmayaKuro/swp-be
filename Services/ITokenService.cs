using swp_be.Data;
using swp_be.Models;

namespace swp_be.Services
{
    public interface ITokenServiceResult
    {
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
    }

    public interface ITokenService
    {
        public ITokenServiceResult CreateToken(User user);
        public ITokenServiceResult Refresh(string refreshToken);
        public bool DeleteToken(string refreshToken);
    }
}
