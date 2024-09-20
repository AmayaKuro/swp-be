using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swp_be.Models
{
    public class Koi
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int KoiID { get; set; }          // Mã cá Koi
        public string Gender { get; set; }         // Giới tính
        public int Age { get; set; }              // Tuổi
        public decimal Size { get; set; }         // Kích thước
        public decimal DailyFeedAmount { get; set; } // Lượng thức ăn/ngày
        public decimal Price { get; set; }         // Giá cá
        public string Personality { get; set; }    // Tính cách
        public string Origin { get; set; }         // Nguồn gốc
        public decimal SelectionRate { get; set; } // Tỷ lệ sàng lọc
        public string Species { get; set; }        // Loài
        public string AddOn { get; set; }          // Phụ kiện kèm theo
    }
}
