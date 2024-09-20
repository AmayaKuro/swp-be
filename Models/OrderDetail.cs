using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swp_be.Models
{
    public class OrderDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderDetailID { get; set; } // Mã chi tiết đơn hàng
        public int KoiID { get; set; }         // Mã cá Koi
        public string Type { get; set; }          // Loại (cá thể hoặc lô)
        public int BatchID { get; set; }       // Mã lô cá
        public decimal Price { get; set; }       // Giá cá
        public int OrderID { get; set; }       // Mã đơn hàng
        public int Quantity { get; set; }         // Số lượng lô
        public Order Order { get; set; }          // Reference to Order
        public Batch Batch { get; set; }          // Reference to Batch
        public Koi Koi { get; set; }              // Reference to Koi
    }
}
