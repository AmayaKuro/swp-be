using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swp_be.Models
{
    public enum ConsignmentType
    {
        Sell=0, //0
        Foster=1,//1
    }
    public enum ConsignmentStatus
    {
        awaitingPayment = 0, //Đợi người ký gửi thanh toán 
        available = 1,//Ca van con ban 
        finished = 2,//Da ban hoac het han nuoi ca
        raising = 3,//Ca dang nuoi
        pending = 4,//Dang cho thanh toan
        negotiate = 5//Da cho thuong luong
    }
    public class Consignment
    {
        public Consignment()
        {
            ConsignmentKois = new List<ConsignmentKoi>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ConsignmentID { get; set; }

        [Required]
        public int CustomerID { get; set; }

        [Required]
        public int ConsignmentPriceListID { get; set; }

        public int? OrderID { get; set; }

        [Required]
        public ConsignmentType Type { get; set; }

        [Required]
        public long FosterPrice { get; set; }

        [Required]
        public ConsignmentStatus Status { get; set; }

        [Required]
        public DateTime CreateAt { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public Customer Customer { get; set; }
        public ConsignmentPriceList ConsignmentPriceList { get; set; }
        public Order? Order { get; set; }

        public ICollection<ConsignmentKoi> ConsignmentKois { get; set; }
    }
}