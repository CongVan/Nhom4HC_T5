using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace User.Login.Areas.User.Models
{
    public class UserModel
    {
        public int TaiKhoanID { get; set; }
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public Boolean NhoTaiKhoan { get; set; }
    }
}