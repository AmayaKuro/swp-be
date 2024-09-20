using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swp_be.Models
{
    public class Batch
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string BatchID { get; set; }     // Mã lô cá
        public string Name { get; set; }        // Tên lô cá
        public decimal Price { get; set; }      // Giá của lô cá
        public string Description { get; set; } // Mô tả lô cá
        public int Quantity { get; set; }       // Số lượng cá trong lô
        public int RemainBatch { get; set; }    // Số lô còn lại
    }
}
