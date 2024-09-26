using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swp_be.Models
{
    public class PaymentMethod
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentMethodID { get; set; }

        [Required, MaxLength(100)]
        public string MethodName { get; set; }

    }
}
