using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swp_be.Models
{
    public class Consignment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ConsignmentID { get; set; } // Mã ký gửi
        public int CustomerID { get; set; }    // Mã khách hàng
        public int KoiID { get; set; }         // Mã cá Koi
        public int BatchID { get; set; }       // Mã lô cá (nếu có)
        public string Type { get; set; }          // Loại ký gửi (online/offline)
        public string Status { get; set; }        // Trạng thái ký gửi
        public Customer Customer { get; set; }    // Reference to Customer
        public Koi Koi { get; set; }              // Reference to Koi
        public Batch Batch { get; set; }          // Reference to Batch
    }
}
