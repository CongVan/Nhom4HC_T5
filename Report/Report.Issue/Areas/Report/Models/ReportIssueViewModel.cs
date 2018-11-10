using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Report.Issue.Areas.Report.Models
{
    public class ReportIssueViewModel
    {
        public int TaiKhoanID { get; set; }
        public string HoTen { get; set; }
        public int ChuaXacNhan { get; set; }
        public int XacNhan { get; set; }
        public int DangXuLy { get; set; }
        public int DaXuLy { get; set; }
    }
}