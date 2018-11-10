using Report.Issue.Areas.Report.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Report.Issue.Areas.Report.Service
{
    public class ReportService
    {
        private readonly string strConn = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
        public List<ReportIssueViewModel> GetAllIssue(int duanId)
        {
            using (var conn = new SqlConnection(strConn))
            {
                using (SqlDataAdapter da = new SqlDataAdapter())
                {
                    da.SelectCommand = new SqlCommand("sp_VanDe_BaoCao", conn);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.Add("@DuAnID", SqlDbType.Int).Value = duanId;
                    DataSet ds = new DataSet();
                    da.Fill(ds, "tableReport");
                    DataTable dt = ds.Tables["tableReport"];

                    var data = new List<ReportIssueModel>();
                    var reult = new List<ReportIssueViewModel>();
                    foreach (DataRow row in dt.Rows)
                    {
                        data.Add(new ReportIssueModel()
                        {
                            TaiKhoanID = Convert.ToInt32(row["TaiKhoanID"].ToString()),
                            HoTen = row["HoTen"].ToString(),
                            TrangThai = Convert.ToInt32(row["TrangThai"].ToString()),
                            SoLuong = Convert.ToInt32(row["SoLuong"].ToString())
                        });
                    }

                    if (data.Count > 0)
                    {
                        var lstThanhVien = from c in data
                                           group c by new { c.TaiKhoanID, c.HoTen } into gsc
                                           select new ReportIssueModel
                                           {
                                               TaiKhoanID = gsc.Key.TaiKhoanID,
                                               HoTen = gsc.Key.HoTen
                                           };
                        foreach (var item in lstThanhVien)
                        {
                            var chuaXacNhan = data.Where(x => x.TaiKhoanID == item.TaiKhoanID && x.TrangThai == 1).DefaultIfEmpty(new ReportIssueModel() { SoLuong = 0 }).FirstOrDefault().SoLuong;
                            var xacNhan = data.Where(x => x.TaiKhoanID == item.TaiKhoanID && x.TrangThai == 2).DefaultIfEmpty(new ReportIssueModel() { SoLuong = 0 }).FirstOrDefault().SoLuong;
                            var dangXuLy = data.Where(x => x.TaiKhoanID == item.TaiKhoanID && x.TrangThai == 3).DefaultIfEmpty(new ReportIssueModel() { SoLuong = 0 }).FirstOrDefault().SoLuong;
                            var daXuLy = data.Where(x => x.TaiKhoanID == item.TaiKhoanID && x.TrangThai == 4).DefaultIfEmpty(new ReportIssueModel() { SoLuong = 0 }).FirstOrDefault().SoLuong;
                            reult.Add(new ReportIssueViewModel()
                            {
                                TaiKhoanID = item.TaiKhoanID,
                                HoTen = item.HoTen,
                                ChuaXacNhan = chuaXacNhan,
                                XacNhan = xacNhan,
                                DangXuLy = dangXuLy,
                                DaXuLy = daXuLy,
                            });
                        }
                    }
                    return reult; 
                }
            }
        }

        public List<DuAnModel> GetAllProject(int taikhoanID)
        {
            using (var conn = new SqlConnection(strConn))
            {
                using (SqlDataAdapter da = new SqlDataAdapter())
                {
                    da.SelectCommand = new SqlCommand("sp_BaoCao_DanhSachDuAn", conn);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.Add("@TaiKhoanID", SqlDbType.Int).Value = taikhoanID;
                    DataSet ds = new DataSet();
                    da.Fill(ds, "result_version");
                    DataTable dt = ds.Tables["result_version"];

                    var data = new List<DuAnModel>();
                    foreach (DataRow row in dt.Rows)
                    {
                        data.Add(new DuAnModel()
                        {
                            ID = Convert.ToInt32(row["ID"].ToString()),
                            TenDuAn = row["TenDuAn"].ToString()
                        });
                    }
                    return data;
                }
            }
        }
    }
}