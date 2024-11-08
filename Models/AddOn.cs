using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swp_be.Models
{
    public class AddOn
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddOnID { get; set; }  // ID của AddOn

        public string? OriginCertificate { get; set; }  // Giấy nguồn gốc xuất xứ

        public string? HealthCertificate { get; set; }  // Giấy kiểm tra sức khỏe

        public string? OwnershipCertificate { get; set; }  // Giấy chứng nhận cá Koi

        public int? KoiID { get; set; } 
        public Koi? Koi { get; set; }  

        public int? ConsignmentKoiID { get; set; }
        public ConsignmentKoi? ConsignmentKoi { get; set; }

        public int? KoiInventoryID { get; set; }
        public KoiInventory? KoiInventory { get; set; }

    }
}
