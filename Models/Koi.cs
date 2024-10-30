using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swp_be.Models
{
    public enum KoiStatus
    {
        Available,
        InOrder,
        Sold
    }

    public class Koi
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int KoiID { get; set; }

        [MaxLength(255)]
        public string? Name { get; set; }

        [MaxLength(10)]
        public string? Gender { get; set; }

        public int? Age { get; set; }

        [MaxLength(15)]
        public string? Size { get; set; }

        [MaxLength(20)]
        public string? Color { get; set; }

        [MaxLength(50)]
        public string? DailyFeedAmount { get; set; }

        [Required]
        public long Price { get; set; }

        [MaxLength(255)]
        public string? Personality { get; set; }

        [MaxLength(255)]
        public string? Origin { get; set; }

        [MaxLength(50)]
        public string? SelectionRate { get; set; }

        [Required, MaxLength(255)]
        public string Species { get; set; }

        public KoiStatus Status { get; set; }

        // Image URL
        public string? Image { get; set; }

        public AddOn AddOn { get; set; }
    }
}
