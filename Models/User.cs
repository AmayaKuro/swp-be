using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swp_be.Models
{
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }    // Mã người dùng
        public string Username { get; set; }  // Tài khoản
        public string Password { get; set; }  // Mk
        public string Name { get; set; }      // Tên người dùng
        public string Email { get; set; }     // Email khách hàng
        public string Phone { get; set; }     // Số điện thoại người dùng
        public string Role { get; set; }      // Chức năng người dùng
        public string Address { get; set; }   // Địa chỉ khách hàng
    }
}
