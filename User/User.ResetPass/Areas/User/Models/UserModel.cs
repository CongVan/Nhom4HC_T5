using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace User.ResetPass.Areas.User.Models
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

        public static ResData ResetMatKhau(string Email, string MatKhau)
        {
            ResData res = new ResData();
            try
            {
                var strConn = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
                using (var conn = new SqlConnection(strConn))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_TaiKhoan_ResetMatKhau", conn))
                    {
                        conn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlParameter ResultID = new SqlParameter();
                        ResultID.ParameterName = "@ResultID";
                        ResultID.Direction = ParameterDirection.Output;
                        ResultID.SqlDbType = SqlDbType.Int;                        

                        SqlParameter ResultDesc = new SqlParameter();
                        ResultDesc.ParameterName = "@ResultDesc";
                        ResultDesc.Direction = ParameterDirection.Output;
                        ResultDesc.SqlDbType = SqlDbType.NVarChar;
                        ResultDesc.Size = 500;

                        cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = Email;
                        cmd.Parameters.Add("@MatKhau", SqlDbType.NVarChar).Value = MatKhau;
                        cmd.Parameters.Add(ResultID);
                        cmd.Parameters.Add(ResultDesc);
                        cmd.ExecuteNonQuery();

                        res.ResultID = Int32.Parse(ResultID.Value.ToString());
                        res.ResultDesc = ResultDesc.Value.ToString();
                    }
                }
            }
            catch(Exception ex)
            {
                res.ResultID = -99;
                res.ResultDesc = ex.Message;
            }

            return res;

        }
    }
}