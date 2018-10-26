using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using User.AccountSetting.Areas.User.Models;

namespace User.AccountSetting.Areas.User.Service
{
    public class UserService
    {
        public string CapNhatTaiKhoan(UserModel model)
        {
            var strConn = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
            using (var conn = new SqlConnection(strConn))
            {
                using (SqlCommand cmd = new SqlCommand("sp_TaiKhoan_CapNhat", conn))
                {
                    conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@TaiKhoanID", SqlDbType.Int).Value = model.TaiKhoanID;
                    cmd.Parameters.Add("@TenDangNhap", SqlDbType.NVarChar).Value = model.TenDangNhap;
                    cmd.Parameters.Add("@HoTen", SqlDbType.NVarChar).Value = model.HoTen;
                    cmd.Parameters.Add("@GioiTinh", SqlDbType.Bit).Value = model.GioiTinh == "1" ? true : false;
                    cmd.Parameters.Add("@NgaySinh", SqlDbType.DateTime).Value = model.NgaySinh;
                    cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = model.Email;
                    var result = cmd.ExecuteNonQuery();
                    return result + string.Empty;
                }
            }
        }
        public UserModel GetInfoUser(int id)
        {
            var strConn = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
            using (var conn = new SqlConnection(strConn))
            {
                using (SqlCommand cmd = new SqlCommand("sp_TaiKhoan_XemThongTin", conn))
                {
                    conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    SqlDataReader reader = cmd.ExecuteReader();
                    var result = new UserModel();
                    if (reader.Read())
                    {
                        result.TaiKhoanID = id;
                        result.TenDangNhap = (String)reader["TenDangNhap"];
                        result.HoTen = (String)reader["HoTen"];
                        result.Email = (String)reader["Email"];
                        result.NgaySinh = (DateTime)reader["NgaySinh"];
                        if ((bool)reader["GioiTinh"] == true)
                        {
                            result.GioiTinh = "Nam";
                        }
                        else
                        {
                            result.GioiTinh = "Nữ";
                        }
                    }
                    return result;
                }
            }
        }
    }
}