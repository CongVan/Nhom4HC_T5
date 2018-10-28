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
    }

    public class ResData
    {
        public int ResultID { get; set; }
        public string ResultDesc { get; set; }
    }
}