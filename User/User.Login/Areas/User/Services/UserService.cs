using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using User.Login.Areas.User.Models;

namespace User.Login.Areas.User.Services
{
    public class UserService
    {
        public string DangNhapTaiKhoan(UserModel model)
        {
            var strConn = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
            using (var conn = new SqlConnection(strConn))
            {
                using (SqlCommand cmd = new SqlCommand("sp_TaiKhoan_DangNhap", conn))
                {
                    conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@TenDangNhap", SqlDbType.NVarChar).Value = model.TenDangNhap;
                    cmd.Parameters.Add("@MatKhau", SqlDbType.NVarChar).Value = model.MatKhau;
                    var result = cmd.ExecuteScalar();
                    return result + string.Empty;
                }
            }
        }
    }
}