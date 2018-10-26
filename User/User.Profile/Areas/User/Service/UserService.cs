using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using User.Profile.Areas.User.Models;
namespace User.Profile.Areas.User.Service
{
    public class UserService
    {
        public UserModel XemThongTinTaiKhoan(int id)
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
                        result.HoTen = (String)reader["HoTen"];
                        result.Email = (String)reader["Email"];
                        result.NgaySinh = (DateTime)reader["NgaySinh"];
                        if((bool)reader["GioiTinh"] == true)
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