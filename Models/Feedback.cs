using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swp_be.Models
{
    public class Feedback
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FeedbackID { get; set; }    // Mã phản hồi
        public int OrderID { get; set; }       // Mã đơn hàng
        public int UserID { get; set; }        // Mã khách hàng (Null nếu cho đăng ẩn danh)
        public int Rating { get; set; }           // Đánh giá (từ 1 đến 5)
        public string Comments { get; set; }      // Phản hồi chi tiết
        public Order Order { get; set; }          // Reference to Order
        public User User { get; set; }            // Reference to User
    }
}
