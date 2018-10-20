using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Version.Areas.Project.Models
{
    public class VersionModel
    {
        public int? Id { get; set; }
        public string TenPhienBan { get; set; }
        public string MoTa { get; set; }
        public int DuAnID { get; set; }
        public string NgayBatDau { get; set; }
        public string NgayKetThuc { get; set; }
        public DateTime? NgayTao { get; set; }
        public int NguoiTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        public bool TinhTrang { get; set; }
    }
}