using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;

namespace swp_be.Models
{
    public class FosterBatch
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FosterBatchID { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public int Quantity { get; set; }

        public string? Species { get; set; }

        public decimal PricePerDay { get; set; } = 0;

        [Required]
        public int FosteringDays { get; set; } // Amount of days the batch is fostered

        [Required]
        public int ConsignmentID { get; set; }

        public Consignment Consignment { get; set; }
    }
}
