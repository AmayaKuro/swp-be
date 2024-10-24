using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swp_be.Models
{
    public enum OrderDetailType
    {
        Koi,
        Batch,
        ConsignmentKoi,
    }
    public class OrderDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderDetailID { get; set; }

        public int? KoiID { get; set; }

        public int? ConsignmentKoiID { get; set; }

        public int? BatchID { get; set; }

        public int? Quantity { get; set; }

        [Required, MaxLength(50)]
        public OrderDetailType Type { get; set; }

        [Required]
        public long Price { get; set; }

        [Required]
        public int OrderID { get; set; }

        public Koi? Koi { get; set; }
        public ConsignmentKoi? ConsignmentKoi { get; set; }
        public Batch? Batch { get; set; }
        public Order Order { get; set; }
    }
}
