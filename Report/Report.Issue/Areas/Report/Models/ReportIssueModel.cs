using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Report.Issue.Areas.Report.Models
{
    public class ReportIssueModel
    {
        public int TaiKhoanID { get; set; }

        public string HoTen { get; set; }

        public int TrangThai { get; set; }

        public int SoLuong { get; set; }
    }
}