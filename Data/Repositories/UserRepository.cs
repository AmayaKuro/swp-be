﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using swp_be.data.Repositories;
using swp_be.Data;
using swp_be.Models;


namespace swp_be.Data.Repositories
{
    public class UserRepository : GenericRepository<User> 
    {
        public UserRepository(ApplicationDBContext context) : base(context)
        {
        }

        public User GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }
    }
}