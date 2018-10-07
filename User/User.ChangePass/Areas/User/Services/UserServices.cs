using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using User.ChangePass.Areas.User.Models;

namespace User.ChangePass.Areas.User.Services
{
    public class UserServices
    {
        public string DoiMatKhau(UserModel model)
        {
            var strConn = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
            using (var conn = new SqlConnection(strConn))
            {
                using (SqlCommand cmd = new SqlCommand("sp_TaiKhoan_DoiMatKhau", conn))
                {
                    conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@TenDangNhap", SqlDbType.NVarChar).Value = model.TenDangNhap;
                    cmd.Parameters.Add("@MatKhau", SqlDbType.NVarChar).Value = model.MatKhau;
                    cmd.Parameters.Add("@MatKhauMoi", SqlDbType.NVarChar).Value = model.MatKhauMoi;
                    var result = cmd.ExecuteNonQuery();
                    return result + string.Empty;
                }
            }
        }

    }
}