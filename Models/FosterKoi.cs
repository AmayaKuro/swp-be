using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swp_be.Models
{
    public class FosterKoi
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FosterKoiID { get; set; }

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

        [MaxLength(255)]
        public string? Personality { get; set; }

        [MaxLength(255)]
        public string? Origin { get; set; }

        [MaxLength(50)]
        public string? SelectionRate { get; set; }

        [Required, MaxLength(255)]
        public string Species { get; set; }

        public decimal PricePerDay { get; set; } = 0;

        [Required]
        public int FosteringDays { get; set; }

        [Required]
        public int ConsignmentID { get; set; }
    
        public Consignment Consignment { get; set; }
    }
}
