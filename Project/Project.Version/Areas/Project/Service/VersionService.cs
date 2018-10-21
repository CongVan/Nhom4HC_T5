using Project.Version.Areas.Project.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Project.Version.Areas.Project.Service
{
    public class VersionService
    {
        private readonly string strConn = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
        public string InsUpdVersion(VersionModel model)
        {
            using (var conn = new SqlConnection(strConn))
            {
                using (SqlCommand cmd = new SqlCommand("sp_PhienBan_ThemSua", conn))
                {
                    conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;
                    cmd.Parameters.Add("@TenPhienBan", SqlDbType.NVarChar).Value = model.TenPhienBan;
                    cmd.Parameters.Add("@MoTa", SqlDbType.NVarChar).Value = model.MoTa;
                    cmd.Parameters.Add("@DuAnID", SqlDbType.Int).Value = model.DuAnID;
                    cmd.Parameters.Add("@NgayBatDau", SqlDbType.DateTime).Value = Convert.ToDateTime(model.NgayBatDau);
                    cmd.Parameters.Add("@NgayKetThuc", SqlDbType.DateTime).Value = Convert.ToDateTime(model.NgayKetThuc);
                    cmd.Parameters.Add("@NgayTao", SqlDbType.DateTime).Value = model.NgayTao;
                    cmd.Parameters.Add("@NguoiTao", SqlDbType.Int).Value = model.NguoiTao;
                    cmd.Parameters.Add("@NgayCapNhat", SqlDbType.DateTime).Value = model.NgayCapNhat;
                    cmd.Parameters.Add("@TinhTrang", SqlDbType.Bit).Value = model.TinhTrang;
                    var result = cmd.ExecuteScalar();
                    return result + string.Empty;
                }
            }
        }
        public string GetNameProject(int id)
        {
            using (var conn = new SqlConnection(strConn))
            {
                using (SqlCommand cmd = new SqlCommand("sp_LayTenDuAn", conn))
                {
                    conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                    var result = cmd.ExecuteScalar();
                    return result + string.Empty;
                }
            }
        }

        public List<VersionModel> GetAllVersion(int duanId)
        {
            using (var conn = new SqlConnection(strConn))
            {
                using (SqlDataAdapter da = new SqlDataAdapter())
                {
                    da.SelectCommand = new SqlCommand("sp_PhienBan_DanhSach", conn);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.Add("@DuAnID", SqlDbType.Int).Value = duanId;
                    DataSet ds = new DataSet();
                    da.Fill(ds, "result_version");

                    DataTable dt = ds.Tables["result_version"];

                    var lstResult = new List<VersionModel>();
                   
                    foreach (DataRow row in dt.Rows)
                    {
                        lstResult.Add(new VersionModel()
                        {
                            Id = Convert.ToInt32(row["Id"].ToString()),
                            TenPhienBan = row["TenPhienBan"].ToString(),
                            MoTa = row["MoTa"].ToString(),
                            DuAnID = Convert.ToInt32(row["DuAnID"].ToString()),
                            NgayBatDau = row["NgayBatDau"].ToString(),
                            NgayKetThuc = row["NgayKetThuc"].ToString(),
                            TinhTrang = Convert.ToBoolean(row["TinhTrang"].ToString())
                        });
                    }
                    return lstResult;
                }
            }
        }
    }
}