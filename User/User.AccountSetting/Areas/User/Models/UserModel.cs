using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace User.AccountSetting.Areas.User.Models
{
    public class UserModel
    {
        public int TaiKhoanID { get; set; }
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public string HoTen { get; set; }
        public string GioiTinh { get; set; }
        public DateTime NgaySinh { get; set; }
        public string Email { get; set; }
        public int LoaiTaiKhoanID { get; set; }
        public DateTime NgayTao { get; set; }
        public int NguoiCapNhat { get; set; }
        public DateTime NgayCapNhat { get; set; }
        public int TinhTrang { get; set; }
    }
}