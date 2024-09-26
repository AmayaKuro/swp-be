using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swp_be.Models
{
    public enum OrderDetailType
    {
        Koi,
        Batch,
        Food,
    }
    public class OrderDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderDetailID { get; set; }

        public int KoiID { get; set; }

        public int BatchID { get; set; }

        public int Quantity { get; set; }

        [Required, MaxLength(50)]
        public OrderDetailType Type { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int OrderID { get; set; }

        public Koi Koi { get; set; }
        public Batch Batch { get; set; }
        public Order Order { get; set; }
    }
}
