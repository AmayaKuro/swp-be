using Microsoft.EntityFrameworkCore;
using swp_be.Models;
using System.Xml.Linq;
using YourNamespace.Models;

namespace swp_be.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<Batch> Batches { get; set; }
        public DbSet<Consignment> Consignments { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<ConsignmentKoi> ConsignmentKois { get; set; }
        public DbSet<Koi> Kois { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Blog> Blogs { get; set; }  
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Staff>()
                .HasOne(staff => staff.User)
                .WithOne()
                .HasForeignKey<Staff>(staff => staff.UserID);

            modelBuilder.Entity<Customer>()
                .HasOne(customer => customer.User)
                .WithOne()
                .HasForeignKey<Customer>(customer => customer.UserID);
            modelBuilder.Entity<Transaction>()
                .HasOne(transaction => transaction.Order)
                .WithMany(order => order.Transactions)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Transaction>()
                .HasOne(transaction => transaction.Consignment)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
