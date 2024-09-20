using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swp_be.Models
{
    public class Order
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderID { get; set; }       // Mã đơn hàng
        public int CustomerID { get; set; }    // Mã khách hàng
        public DateTime Date { get; set; }        // Ngày đặt hàng
        public decimal TotalAmount { get; set; }  // Tổng số tiền đơn hàng
        public string Status { get; set; }        // Trạng thái đơn hàng
        public int StaffID { get; set; }       // Mã nhân viên
        public Customer Customer { get; set; }    // Reference to Customer
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public User Staff { get; set; }
    }
}
