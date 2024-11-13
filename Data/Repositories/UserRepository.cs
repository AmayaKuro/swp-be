using System.Collections.Generic;
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

        public bool UpdateProfile(User user)
        {
            var tracker = _context.Attach(user);
            tracker.State = EntityState.Modified;

            tracker.Property(u => u.Password).IsModified = false;
            tracker.Property(u => u.UserID).IsModified = false;

            return true;
        }

        public bool UpdatePassword (User user)
        {
            var tracker = _context.Attach(user);

            tracker.Property(u => u.Password).IsModified = true;

            return true;
        }

        public async Task<Customer> GetCustomerByID(int id)
        {
            return await _context.Customers
        .Include(c => c.User) 
        .FirstOrDefaultAsync(c => c.UserID == id);
        }

        public async Task<bool> UpdateCustomer(Customer customer)
        {
            try
            {
                var tracker = _context.Attach(customer);
                tracker.State = EntityState.Modified;

                 await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log exception or handle it as needed
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
