using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swp_be.Models
{
    public class Batch
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BatchID { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public long PricePerBatch { get; set; }

        public string? Description { get; set; }

        public string? Image { get; set; }

        [Required]
        public int QuantityPerBatch { get; set; }

        [Required]
        public int RemainBatch { get; set; }

        public string? Species { get; set; }

    }
}
