using Microsoft.EntityFrameworkCore;
using swp_be.Models;

namespace swp_be.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<swp_be.Models.User> Users { get; set; }
        public DbSet<swp_be.Models.Customer> Customers { get; set; }
        public DbSet<swp_be.Models.Koi> Kois { get; set; }
        public DbSet<swp_be.Models.Batch> Batches { get; set; }
        public DbSet<swp_be.Models.Order> Orders { get; set; }
        public DbSet<swp_be.Models.OrderDetail> OrderDetails { get; set; }
        public DbSet<swp_be.Models.Promotion> Promotions { get; set; }
        public DbSet<swp_be.Models.Consignment> Consignments { get; set; }
    }
}
