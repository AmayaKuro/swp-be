using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swp_be.Models
{
    public enum ConsignmentType
    {
        Sell, //0
        Foster,//1
    }
    public enum ConsigmentStatus
    {
        available,//Ca van con ban 
        finished,//Da ban hoac het han nuoi ca
        raising,//Ca dang nuoi
        penidng,//Dang cho xac nhan
    }

    public class Consignment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ConsignmentID { get; set; }

        [Required]
        public int CustomerID { get; set; }

        [Required]
        public ConsignmentType Type { get; set; }
        [Required]
        public decimal FosterPrice { get; set; }
        public ConsigmentStatus Status { get; set; }
        public Customer Customer { get; set; }
        public ICollection<FosterBatch> FosterBatches { get; set; }
        public ICollection<ConsignmentKoi> FosterKois { get; set; }
    }
}