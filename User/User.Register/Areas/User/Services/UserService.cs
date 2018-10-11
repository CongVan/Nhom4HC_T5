using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using User.Register.Areas.User.Models;

namespace User.Register.Areas.User.Services
{
    public class UserService
    {
        public string DangKyTaiKhoan(UserModel model)
        {
            var strConn = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
            using (var conn = new SqlConnection(strConn))
            {
                using (SqlCommand cmd = new SqlCommand("sp_TaiKhoan_DangKy", conn))
                {
                    conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@TenDangNhap", SqlDbType.NVarChar).Value = model.TenDangNhap;
                    cmd.Parameters.Add("@MatKhau", SqlDbType.NVarChar).Value = model.MatKhau;
                    cmd.Parameters.Add("@HoTen", SqlDbType.NVarChar).Value = model.HoTen;
                    cmd.Parameters.Add("@GioiTinh", SqlDbType.Bit).Value = model.GioiTinh == "1" ? true : false;
                    cmd.Parameters.Add("@NgaySinh", SqlDbType.DateTime).Value = model.NgaySinh;
                    cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = model.Email;
                    cmd.Parameters.Add("@LoaiTaiKhoanID", SqlDbType.Int).Value = model.LoaiTaiKhoanID;
                    cmd.Parameters.Add("@NguoiTao", SqlDbType.NVarChar).Value = null;
                    cmd.Parameters.Add("@NgayTao", SqlDbType.DateTime).Value = model.NgayTao;
                    cmd.Parameters.Add("@TinhTrang", SqlDbType.Int).Value = model.TinhTrang;
                    var result = cmd.ExecuteScalar();
                    return result + string.Empty;
                }
            }
        }
        public int KiemTraTenDangNhap(string tendangnhap)
        {
            var strConn = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
            using (var conn = new SqlConnection(strConn))
            {
                using (SqlCommand cmd = new SqlCommand("sp_TaiKhoan_CheckTenDangNhap", conn))
                {
                    conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@TenDangNhap", SqlDbType.NVarChar).Value = tendangnhap;
                    var result = cmd.ExecuteScalar();
                    return (int)result;
                }
            }
        }

        public int KiemTraEmail(string email)
        {
            var strConn = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
            using (var conn = new SqlConnection(strConn))
            {
                using (SqlCommand cmd = new SqlCommand("sp_TaiKhoan_CheckEmail", conn))
                {
                    conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = email;
                    var result = cmd.ExecuteScalar();
                    return (int)result;
                }
            }
        }

    }
}