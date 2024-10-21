using swp_be.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourNamespace.Models
{
    public class Blog
    {
        [Key]
        public int BlogId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public String BlogSlug { get; set; }
        public String Description { get; set; }

        public String Content {  get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime UpdateAt { get; set; }
        public User User { get; set; }

        public int UserID { get; set; }
    }
}
