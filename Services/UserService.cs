﻿using Microsoft.EntityFrameworkCore;
using swp_be.Models;
using swp_be.Data;
using Zxcvbn;
using BC = BCrypt.Net.BCrypt;

namespace swp_be.Services
{
    public class UserServiceResult : IUserServiceResult
    {
        public bool success { get; set; } = true;
        public string message { get; set; } = "success";
        public User? userInfo { get; set; }
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
                result.success = false;
                result.message = "User not found";

                return result;
            }

            if (!BC.Verify(user.Password, info.Password))
            {
                result.success = false;
                result.message = "Invalid password";

                return result;
            }

            //Login successfully
            result.success = true;
            result.message = "Login successful";
            result.userInfo = info;

            return result;
        }

        public IUserServiceResult Register(User user)
        {
            UserServiceResult result = new UserServiceResult();

            if (unitOfWork.UserRepository.GetUserByUsername(user.Username) != null)
            {
                result.success = false;
                result.message = "Username already exists";

                return result;
            }


            Result zxcvbnResult = Zxcvbn.Core.EvaluatePassword(user.Password);

            if (zxcvbnResult.Score < 3)
            {
                result.success = false;
                result.message = "Weak password";
                return result;
            }

            user.Password = BC.HashPassword(user.Password);

            // Prepare user for creating
            user.UserID = 0;
            user.Name = user.Name ?? user.Username;
            user.Role = Role.Customer;

            unitOfWork.UserRepository.Create(user);
            unitOfWork.Save();

            // Add user to result
            result.userInfo = user;

            return result;
        }
    }
}