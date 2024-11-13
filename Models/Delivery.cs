using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swp_be.Models
{
    public enum DeliveryStatus
    {
        Delivering = 0,    
        Delivered = 1,     // Đã giao hàng thành công
        Failed = 2,        // Giao hàng thất bại
        Cancelled = 3      // Đơn hàng bị hủy
    }

    public class Delivery
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeliveryID { get; set; }

        [Required]
        public int OrderID { get; set; }

        [Required]
        public int CustomerID { get; set; }

        public string? Address { get; set; }

        public DeliveryStatus Status { get; set; }

        [Required]
        public DateTime StartDeliDay { get; set; }

        public DateTime? EndDeliDay { get; set; }

        public string? Reason { get; set; }

        public string? ReasonImage { get; set; }

        public Order Order { get; set; }  // Navigation Property

     
        public Customer Customer { get; set; }  // Navigation Property


    }
}
