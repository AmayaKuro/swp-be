using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swp_be.Models
{
    public class Token
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string TokenID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public string RefreshToken { get; set; }

        [Required]
        public DateTime CreateAt { get; set; } // Date for Refresh token creation

        [Required]
        public DateTime ExpireAt { get; set; } // Date for Refresh token expiration

        public User User { get; set; }
    }
}
