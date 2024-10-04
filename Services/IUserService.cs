using swp_be.Data;
using swp_be.Models;

namespace swp_be.Services
{
    public interface IUserServiceResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public interface IUserService
    {
        public IUserServiceResult Login(User user);
        public IUserServiceResult Register(User user);
    }
}
