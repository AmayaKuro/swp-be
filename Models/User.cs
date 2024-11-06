using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Text.Json.Serialization;

namespace swp_be.Models
{
    public enum Role
    {
        Admin,
        Staff,
        Customer
    }

    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        [Required, MaxLength(50)]
        public string Username { get; set; }

        [Required, MaxLength(255)]
        public string Password { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; }
        
        [MaxLength(255)]
        public string? Email { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        public string? Address { get; set; }

        [Required]
        public Role Role { get; set; }
    }
}
