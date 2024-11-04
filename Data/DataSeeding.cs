using swp_be.Models;
using swp_be.Utils;
using BC = BCrypt.Net.BCrypt;

namespace swp_be.Data
{
    // In case of seeding data got delete when add migrate
    //modelBuilder.Entity<PaymentMethod>()
    //            .HasData(DataSeeding.PaymentMethods);
    //modelBuilder.Entity<User>()
    //            .HasData(DataSeeding.Users);
    //modelBuilder.Entity<Staff>()
    //            .HasData(DataSeeding.Staffs);
    //modelBuilder.Entity<Customer>()
    //            .HasData(DataSeeding.Customers);
    public class DataSeeding
    {
        public static List<PaymentMethod> PaymentMethods { get; } = new List<PaymentMethod>()
        {
           new PaymentMethod() {
                PaymentMethodID=1,
                MethodName="Cash",
           },
           new PaymentMethod() {
                PaymentMethodID=2,
                MethodName="VNPay",
           },
        };

        public static List<ConsignmentPriceList> Consignments { get; } = new List<ConsignmentPriceList>()
        {
            new ConsignmentPriceList()
            {
                ConsignmentPriceListID = 1,
                ConsignmentPriceName = "gay tra nhieu tien",
                PricePerDay = 100000,
            },
        };

        public static List<User> Users { get; } = new List<User>()
        {
            new User()
            {
                UserID = 3001,
                Username = "admin",
                Password = BC.HashPassword("admin"),
                Name = "Admin",
                Role = Role.Admin,
            },
            new User()
            {
                UserID = 3002,
                Username = "staff",
                Password = BC.HashPassword("staff"),
                Name = "Staff",
                Role = Role.Staff,
            },
            new User()
            {
                UserID = 3003,
                Username = "customer",
                Password = BC.HashPassword("customer"),
                Name="Customer",
                Role = Role.Customer,
            },
            new User()
            {
                UserID = 3004,
                Username = "string",
                Password = BC.HashPassword("string"),
                Name="String",
                Role = Role.Admin,
            },
        };

        public static List<Staff> Staffs { get; } = new List<Staff>()
        {
            new Staff()
            {
                UserID = Users[0].UserID,
            },
            new Staff()
            {
                UserID = Users[1].UserID,
            },
        };

        public static List<Customer> Customers { get; } = new List<Customer>()
        {
            new Customer()
            {
                UserID = Users[2].UserID,
            },
            new Customer()
            {
                UserID = Users[3].UserID,
            },
        };
    }
}
