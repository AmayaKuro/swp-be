using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swp_be.Models
{
    public enum ConsignmentType
    {
        Sell,
        Foster,
    }

    public class Consignment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ConsignmentID { get; set; }

        [Required]
        public int CustomerID { get; set; }

        [Required]
        public ConsignmentType Type { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        public Customer Customer { get; set; }
    }
}
