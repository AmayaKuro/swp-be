using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swp_be.Models
{
    public class Promotion
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PromotionID { get; set; }

        [Required, MaxLength(50)]
        public string Code { get; set; }

        public string? Description { get; set; }

        [Required]
        public decimal DiscountRate { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? RemainingRedeem { get; set; }
    }
}
