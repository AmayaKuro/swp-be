using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swp_be.Models
{
    public class Staff
    {
        [Key]
        public int UserID { get; set; }       // Mã người dùng

        public string? StaffZone {  get; set; }

        public User User { get; set; }           // Reference to User
    }
}
