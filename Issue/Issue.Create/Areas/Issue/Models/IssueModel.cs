using Issue.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Issue.Create.Areas.Issue.Models
{
    public class IssueModel
    {
        public int VanDeID { get; set; }
        public string TenVanDe { get; set; }
        public int DuAnID { get; set; }
        public int LoaiVanDeID { get; set; }
        public string MoTa { get; set; }
        public string FileDinhKem { get; set; }
        public int TrangThai { get; set; }
        public string NguoiThucHien { get; set; }
        public string NgayBatDau { get; set; }
        public string NgayKetThuc { get; set; }
        public int SoGioDuKien { get; set; }
        public int SoGioThucTe { get; set; }
        public string NguoiTao { get; set; }
        public string NguoiCapNhat { get; set; }
        public int TinhTrang { get; set; }

        public IssueModel()
        {
            this.DuAnID = 0;
        }

        public static IssueModel LayThongTinVanDe(int VanDeID)
        {
            IssueModel IM = new IssueModel();
            try {

                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString(), CommandType.Text, "SELECT * FROM VanDe WHERE VanDeID=" + VanDeID.ToString());
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    IM.VanDeID = Int32.Parse(ds.Tables[0].Rows[0]["VanDeID"].ToString());
                    IM.DuAnID = Int32.Parse(ds.Tables[0].Rows[0]["DuAnID"].ToString());
                    IM.LoaiVanDeID = Int32.Parse(ds.Tables[0].Rows[0]["LoaiVanDeID"].ToString());
                    IM.TenVanDe = ds.Tables[0].Rows[0]["TenVanDe"].ToString();
                    IM.MoTa = ds.Tables[0].Rows[0]["MoTa"].ToString();
                    IM.TrangThai = Int32.Parse(ds.Tables[0].Rows[0]["TrangThai"].ToString());
                    IM.NguoiThucHien = ds.Tables[0].Rows[0]["NguoiThucHien"].ToString();
                    IM.NgayBatDau = FormatDate(ds.Tables[0].Rows[0]["NgayBatDau"].ToString());
                    IM.NgayKetThuc = FormatDate(ds.Tables[0].Rows[0]["NgayKetThuc"].ToString());
                    IM.SoGioDuKien = Int32.Parse(ds.Tables[0].Rows[0]["SoGioDuKien"].ToString());
                    IM.SoGioThucTe = Int32.Parse(ds.Tables[0].Rows[0]["SoGioThucTe"].ToString());
                }
            }
            catch (Exception ex)
            { }

            return IM;
        }

        public static List<CBXModel> LayDanhSachTaiKhoan()
        {
            List<CBXModel> lst = new List<CBXModel>();
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString(), CommandType.Text, "SELECT TaiKhoanID, TenDangNhap FROM TaiKhoan WHERE TinhTrang=1");
                if (ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        CBXModel CBX = new CBXModel();
                        CBX.Value = "txtNguoiThucHien";
                        CBX.ID = Int32.Parse(ds.Tables[0].Rows[i]["TaiKhoanID"].ToString());
                        CBX.Name = ds.Tables[0].Rows[i]["TenDangNhap"].ToString();

                        lst.Add(CBX);
                    }

                }
            }
            catch (Exception ex)
            { }

            return lst;
        }

        public static List<CBXModel> LayDanhSachDuAn()
        {
            List<CBXModel> lst = new List<CBXModel>();
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString(), CommandType.Text, "SELECT DuAnID, TenDuAn FROM DuAn");
                if (ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        CBXModel CBX = new CBXModel();
                        CBX.Value = "DuAnID";
                        CBX.ID = Int32.Parse(ds.Tables[0].Rows[i]["DuAnID"].ToString());
                        CBX.Name = ds.Tables[0].Rows[i]["TenDuAn"].ToString();

                        lst.Add(CBX);
                    }

                }
            }
            catch (Exception ex)
            { }

            return lst;
        }

        public static ResData CapNhatVanDe(IssueModel IM)
        {
            ResData res = new ResData();
            try
            {
                SqlParameter ResultID = new SqlParameter();
                ResultID.ParameterName = "@ResultID";
                ResultID.Direction = ParameterDirection.Output;
                ResultID.SqlDbType = SqlDbType.Int;

                SqlParameter ResultDesc = new SqlParameter();
                ResultDesc.ParameterName = "@ResultDesc";
                ResultDesc.Direction = ParameterDirection.Output;
                ResultDesc.SqlDbType = SqlDbType.NVarChar;
                ResultDesc.Size = 500;

                SqlParameter[] pars = {
                    new SqlParameter ("VanDeID", IM.VanDeID),
                    new SqlParameter ("DuAnID", IM.DuAnID),
                    new SqlParameter ("LoaiVanDeID", IM.LoaiVanDeID),
                    new SqlParameter ("TenVanDe", IM.TenVanDe),
                    new SqlParameter ("MoTa", IM.MoTa),
                    new SqlParameter ("FileDinhKem", IM.FileDinhKem),
                    new SqlParameter ("TrangThai", IM.TrangThai),
                    new SqlParameter ("NguoiThucHien", IM.NguoiThucHien),
                    new SqlParameter ("NgayBatDau", IM.NgayBatDau),
                    new SqlParameter ("NgayKetThuc", IM.NgayKetThuc),
                    new SqlParameter ("SoGioDuKien", IM.SoGioDuKien),
                    new SqlParameter ("SoGioThucTe", IM.SoGioThucTe),
                    new SqlParameter ("NguoiCapNhat", IM.NguoiCapNhat),
                    ResultID,
                    ResultDesc
                };

                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString(), CommandType.StoredProcedure, "sp_VanDe_Update", pars);

                res.ResultID = Int32.Parse(ResultID.Value.ToString());
                res.ResultDesc = ResultDesc.Value.ToString();

            }
            catch (Exception ex)
            {
                res.ResultID = -99;
                res.ResultDesc = ex.Message;
            }
            return res;
        }

        public static string FormatDate(string str)
        {
            string[] mdy = str.Substring(0,10).Split('/');
            return mdy[1] + "/" + mdy[0] + "/" + mdy[2];
        }
    }

    public class ResData
    {
        public int ResultID { get; set; }
        public string ResultDesc { get; set; }
    }

    public class CBXModel
    {
        public string Value { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
    }
    
}