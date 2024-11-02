using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swp_be.Models
{
    public class Customer
    {
        [Key]
        public int UserID { get; set; }       // Mã người dùng
        public int LoyaltyPoints { get; set; } = 0;   // Điểm tích lũy của khách hàng
        public User User { get; set; }           // Reference to User
        public ICollection<KoiInventory>? KoiInventories { get; set; }

    }
}
