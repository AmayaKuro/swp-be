using swp_be.Data;
using swp_be.Models;

namespace swp_be.Services
{
    public interface IUserServiceResult
    {
        public bool success { get; set; }
        public string message { get; set; }
        public User? userInfo { get; set; }
    }

    public interface IUserService
    {
        public IUserServiceResult Login(User user);
        public IUserServiceResult Register(User user);
        public User GetUserProfile(int id);
    }
}
