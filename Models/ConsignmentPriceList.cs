using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swp_be.Models
{
    public class ConsignmentPriceList
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ConsignmentPriceListID { get; set; }

        public string? ConsignmentPriceName { get; set; }
        [Required]
        public long PricePerDay { get; set; }

        public ICollection<Consignment> Consignments { get; set; } = new List<Consignment>();
    }
}


