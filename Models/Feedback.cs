using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swp_be.Models
{
    public class Feedback
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FeedbackID { get; set; }

        [Required]
        public int OrderID { get; set; }

        [Required]
        public int? CustomerID { get; set; }

        [Required, Range(1, 5)]
        public int Rating { get; set; }

        public string? Comment { get; set; }

        [Required]
        public DateTime DateFb { get; set; }

        public Order? Order { get; set; }
        public Customer? Customer { get; set; }
    }
}
