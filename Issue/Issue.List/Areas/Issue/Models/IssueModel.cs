using Issue.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Issue.List.Areas.Issue.Models
{
    public class IssueModel
    {
        public int STT { get; set; }
        public int VanDeID { get; set; }
        public string TenVanDe { get; set; }
        public int DuAnID { get; set; }
        public string TenDuAn { get; set; }
        public int LoaiVanDeID { get; set; }
        public string MoTa { get; set; }
        public string FileDinhKem { get; set; }
        public int TrangThai { get; set; }
        public string TrangThaiDesc { get; set; }
        public string NguoiThucHien { get; set; }
        public string TenDangNhap { get; set; }
        
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

        public static List<IssueModel> GetList(string TenDangNhap)
        {
            List<IssueModel> lst = new List<IssueModel>();
            try
            {

                SqlParameter[] pars = {
                    new SqlParameter ("@TenDangNhap", TenDangNhap)
                };

                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString(), CommandType.StoredProcedure, "sp_VanDe_LayDanhSach", pars);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        IssueModel IM = new IssueModel();
                        IM.STT = Int32.Parse(dr["STT"].ToString());
                        IM.VanDeID = Int32.Parse(dr["VanDeID"].ToString());
                        IM.DuAnID = Int32.Parse(dr["DuAnID"].ToString());
                        IM.TenDuAn = dr["TenDuAn"].ToString();
                        IM.TenVanDe = dr["TenVanDe"].ToString();
                        IM.MoTa = dr["MoTa"].ToString();
                        IM.TrangThai = Int32.Parse(dr["TrangThai"].ToString());
                        IM.TrangThaiDesc = dr["TrangThaiDesc"].ToString();
                        IM.NguoiThucHien = dr["NguoiThucHien"].ToString();
                        IM.TenDangNhap = dr["TenDangNhap"].ToString();
                        IM.NgayBatDau = dr["NgayBatDau"].ToString();
                        IM.NgayKetThuc = dr["NgayKetThuc"].ToString();
                        IM.SoGioDuKien = Int32.Parse(dr["SoGioDuKien"].ToString());
                        IM.SoGioThucTe = Int32.Parse(dr["SoGioThucTe"].ToString());
                        IM.TinhTrang = Int32.Parse(dr["TinhTrang"].ToString());

                        lst.Add(IM);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return lst;
        }

        public static ResData CapNhatTinhTrang(int VanDeID, string TenDangNhap)
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
                    new SqlParameter ("VanDeID", VanDeID),
                    new SqlParameter ("TenDangNhap", TenDangNhap),
                    ResultID,
                    ResultDesc
                };

                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString(), CommandType.StoredProcedure, "sp_VanDe_CapNhatTinhTrang", pars);

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
    }

    public class ResData
    {
        public int ResultID { get; set; }
        public string ResultDesc { get; set; }
    }
}