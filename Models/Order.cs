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

    public class Order
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderID { get; set; }

        [Required]
        public int CustomerID { get; set; }

        [Required]
        public int StaffID { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        [Required, MaxLength(50)]
        public OrderStatus Status { get; set; }

        public int? PromotionID { get; set; }

        [DeleteBehavior(DeleteBehavior.Restrict)]
        public Customer Customer { get; set; }  // Navigation Property
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public Staff Staff { get; set; }  // Navigation Property
        public Promotion Promotion { get; set; }  // Navigation Property

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
