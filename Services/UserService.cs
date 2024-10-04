using Microsoft.EntityFrameworkCore;
using swp_be.Models;
using swp_be.Data;
using Zxcvbn;
using BC = BCrypt.Net.BCrypt;

namespace swp_be.Services
{
    public class UserServiceResult : IUserServiceResult
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "success";
    }

    public class UserService : IUserService
    {
        // Create a service class for Koi in repository + service pattern
        private ApplicationDBContext _context;
        private readonly UnitOfWork unitOfWork;

        public UserService(ApplicationDBContext _context)
        {
            this._context = _context;
            this.unitOfWork = new UnitOfWork(_context);
        }

        public IUserServiceResult Login(User user)
        {
            UserServiceResult result = new UserServiceResult();

            User info = unitOfWork.UserRepository.GetUserByUsername(user.Username);

            if (info == null)
            {
                result.Success = false;
                result.Message = "User not found";

                return result;
            }

            if (!BC.Verify(user.Password, info.Password))
            {
                result.Success = false;
                result.Message = "Invalid password";

                return result;
            }

            //Login successfully
            result.Success = true;
            result.Message = "Login successful";

            return result;
        }

        public IUserServiceResult Register(User user)
        {
            UserServiceResult result = new UserServiceResult();

            if (unitOfWork.UserRepository.GetUserByUsername(user.Username) != null)
            {
                result.Success = false;
                result.Message = "Username already exists";

                return result;
            }


            Result zxcvbnResult = Zxcvbn.Core.EvaluatePassword(user.Password);

            if (zxcvbnResult.Score < 3)
            {
                result.Success = false;
                result.Message = "Weak password";
                return result;
            }

            user.Password = BC.HashPassword(user.Password);

            unitOfWork.UserRepository.Create(user);
            unitOfWork.Save();

            return result;
        }
    }
}
