using Microsoft.EntityFrameworkCore;
using swp_be.Models;
using System.Xml.Linq;


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
        public DbSet<KoiInventory> KoiInventory { get; set; }
        public DbSet<ConsignmentPriceList> ConsignmentPriceLists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ConsignmentKoi>()
                .HasOne(ck => ck.AddOn)
                .WithOne(a => a.ConsignmentKoi)
                .HasForeignKey<ConsignmentKoi>(ck => ck.AddOnId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Koi>()
                .HasOne(k => k.AddOn)
                .WithOne(a => a.Koi)
                .HasForeignKey<Koi>(k => k.AddOnId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<KoiInventory>()
                .HasOne(ki => ki.AddOn)
                .WithOne(a => a.KoiInventory)
                .HasForeignKey<KoiInventory>(ki => ki.AddOnId)
                .OnDelete(DeleteBehavior.Cascade);

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

            modelBuilder.Entity<User>()
                .HasData(DataSeeding.Users);
            modelBuilder.Entity<PaymentMethod>()
                .HasData(DataSeeding.PaymentMethods);
            modelBuilder.Entity<Staff>()
                .HasData(DataSeeding.Staffs);
            modelBuilder.Entity<Customer>()
                .HasData(DataSeeding.Customers);
            modelBuilder.Entity<ConsignmentPriceList>()
                .HasData(DataSeeding.Consignments);
        }
    }
}
