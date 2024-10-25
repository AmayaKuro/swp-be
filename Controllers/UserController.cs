using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using swp_be.Data;
using swp_be.Data.Repositories;
using swp_be.Models;
using swp_be.Services;

namespace swp_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly UserService userService;
        private readonly TokenService tokenService;

        public UserController(ApplicationDBContext context)
        {
            this._context = context;
            this.userService = new UserService(context);
            this.tokenService = new TokenService(context);
        }

        // GET: api/login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            IUserServiceResult result = userService.Login(user);

            if (!result.success)
            {
                return BadRequest(new
                {
                    message = result.message
                });
            }

            ITokenServiceResult token = tokenService.CreateToken(result.userInfo);

            return Ok(new
            {
                token,
                result.userInfo.Role,
                result.userInfo.Name,
            });
        }

        // POST: api/register
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(User user)
        {
            IUserServiceResult result = userService.Register(user);

            if (!result.success)
            {
                return BadRequest(new
                {
                    message = result.message
                });
            }

            ITokenServiceResult token = tokenService.CreateToken(result.userInfo);

            return Ok(new
            {
                token,
                result.userInfo.Role,
                result.userInfo.Name,
            });
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh([FromBody] string refreshToken)
        {
            ITokenServiceResult result = tokenService.Refresh(refreshToken);

            if (result == null) return Unauthorized();

            return Ok(result);
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout([FromBody] string refreshToken)
        {
            bool success = tokenService.DeleteToken(refreshToken);

            return Ok();
        }

        [HttpGet]
        [Route("profile")]
        [Authorize("all")]
        public async Task<IActionResult> Profile()
        {
            int userID = int.Parse(User.FindFirstValue("userID"));

            User user = userService.GetUserProfile(userID);

            return Ok(user);
        }

        [HttpGet]
        [Route("profile/all")]
        [Authorize("staff, admin")]
        public async Task<IActionResult> AllProfile(int id)
        {
            List<User> user = userService.GetAllUserProfile();

            return Ok(user);
        }

        [HttpGet]
        [Route("profile/{id}")]
        [Authorize("staff, admin")]
        public async Task<IActionResult> ProfileByID(int id)
        {
            User user = userService.GetUserProfile(id);

            return Ok(user);
        }

        [HttpPut]
        [Route("edit")]
        [Authorize("all")]
        public async Task<IActionResult> EditProfile([FromBody] User editUser)
        {
            int userID = int.Parse(User.FindFirstValue("userID"));

            // Checkk if admin or same id
            if (!User.IsInRole("Admin") && userID != editUser.UserID)
            {
                return Forbid();
            }

            userService.UpdateUserProfile(editUser);

            return Ok();
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [Authorize("all")]
        public async Task<IActionResult> DeleteProfile(int id)
        {
            int userID = int.Parse(User.FindFirstValue("userID"));

            // Checkk if admin or same id
            if (!User.IsInRole("Admin") && userID != id)
            {
                return Forbid();
            }
            userService.DeleteUserProfile(id);

            return Ok();
        }

    }
}
