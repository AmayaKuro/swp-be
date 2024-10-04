using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public UserController(ApplicationDBContext context)
        {
            this._context = context;
            this.userService = new UserService(context);
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

            if (!result.Success)
            {
                return BadRequest(new {
                    message = result.Message
                });
            }

            return Ok();
        }

        // POST: api/register
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(User user)
        {
            IUserServiceResult result = userService.Register(user);

            if (!result.Success)
            {
                return BadRequest(new
                {
                    message = result.Message
                });
            }

            return Ok();
        }

    }
}
