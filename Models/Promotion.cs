using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swp_be.Models
{
    public class Promotion
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PromotionID { get; set; }   // Mã khuyến mãi
        public string Description { get; set; }   // Mô tả khuyến mãi
        public decimal DiscountRate { get; set; } // Tỷ lệ giảm giá (%)
        public DateTime StartDate { get; set; }   // Ngày bắt đầu khuyến mãi
        public DateTime EndDate { get; set; }     // Ngày kết thúc khuyến mãi
    }
}
