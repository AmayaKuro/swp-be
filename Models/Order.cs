using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swp_be.Models
{

    /// <summary>
    /// Represents the status of an order.
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// The order is pending for payment.
        /// </summary>
        Pending,

        /// <summary>
        /// The order is completed.
        /// </summary>
        Completed,

        /// <summary>
        /// The order is cancelled.
        /// </summary>
        Cancelled,
    }

    public enum OrderType
    {
        Online,
        // Đặt cọc 50% giá trị đơn hàng, sau đó thanh toán phần còn lại bằng tiền mặt khi nhận hàng
        Offline,
    }

    public class Order
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderID { get; set; }

        public int? CustomerID { get; set; }

        public int? StaffID { get; set; }

        [Required]
        public DateTime CreateAt { get; set; }

        public DateTime? UpdateAt { get; set; }

        [Required]
        public long TotalAmount { get; set; }

        [Required]
        public OrderType Type { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        public int? PromotionID { get; set; }

        public string? Reason { get; set; }

        public string? ReasonImage { get; set; }

        [DeleteBehavior(DeleteBehavior.SetNull)]
        public Customer? Customer { get; set; }  // Navigation Property

        public Promotion? Promotion { get; set; }  // Navigation Property

        [DeleteBehavior(DeleteBehavior.ClientSetNull)]
        public Staff? Staff { get; set; }  // Navigation Property

        public ICollection<OrderDetail> OrderDetails { get; set; }
        // Order will have 1 transactions if online or 2 transactions if offline (deposit and final payment)
        public ICollection<Transaction> Transactions { get; set; }

        public Order()
        {
            OrderDetails = new List<OrderDetail>();
            Transactions = new List<Transaction>();
        }
    }
}
