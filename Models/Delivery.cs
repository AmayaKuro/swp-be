using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swp_be.Models
{
    public class Delivery
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeliveryID { get; set; }

        [Required]
        public int OrderID { get; set; }

        [Required]
        public int CustomerID { get; set; }

        [Required, MaxLength(50)]
        public string Status { get; set; }

        [Required]
        public DateTime StartDeliDay { get; set; }

        public DateTime? EndDeliDay { get; set; }

        public Order Order { get; set; }  // Navigation Property

     
        public Customer Customer { get; set; }  // Navigation Property


    }
}
