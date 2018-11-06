using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Management.Areas.Project.Controllers
{
    public class ProjectController : Controller
    {
        static string cnnString = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;

        //
        // GET: /Project/Project/

        public ActionResult Project()
        {
            if (Session["UserID"] == null)
            {
                return Redirect("/User/Login");
            }
            ViewBag.UserID = Session["UserID"].ToString();


            return View();
        }

        [HttpPost]
        public ActionResult AddProject()
        {
            try
            {
                using (var cnn = new SqlConnection(cnnString))
                {
                    using (var cmd = new SqlCommand("sp_Tao_DuAn", cnn))
                    {
                        cnn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TenDuAn", Request["TenDuAn"]);
                        cmd.Parameters.AddWithValue("@MoTa", Request["MoTa"]);
                        cmd.Parameters.AddWithValue("@TruongDuAnID", Request["TruongDuAnID"]);
                        cmd.Parameters.AddWithValue("@NguoiTaoID", Request["NguoiTaoID"] ?? "0");
                        int c = cmd.ExecuteNonQuery();
                        return Json(new { result = c, msg = "OK" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = -1, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult UpdateProject()
        {
            try
            {
                using (var cnn = new SqlConnection(cnnString))
                {
                    using (var cmd = new SqlCommand("sp_CapNhatDuAn", cnn))
                    {
                        cnn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@DuAnID", Request["DuAnID"]);
                        cmd.Parameters.AddWithValue("@TenDuAn", Request["TenDuAn"]);
                        cmd.Parameters.AddWithValue("@MoTa", Request["MoTa"]);
                        cmd.Parameters.AddWithValue("@TruongDuAnID", Request["TruongDuAnID"]);
                        cmd.Parameters.AddWithValue("@NguoiCapNhatID", Request["NguoiCapNhatID"] ?? "0");
                        int c = cmd.ExecuteNonQuery();
                        return Json(new { result = c, msg = "OK" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = -1, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult CheckKey(string key)
        {
            try
            {
                using (var cnn = new SqlConnection(cnnString))
                {
                    using (var cmd = new SqlCommand("SELECT ID,Khoa FROM DuAn WHERE Khoa='" + key + "'", cnn))
                    {
                        cnn.Open();
                        var adpt = new SqlDataAdapter(cmd);
                        var tb = new DataTable();
                        adpt.Fill(tb);
                        var results = tb.AsEnumerable().Select(c => new
                        {
                            ID = c["ID"],
                            Key = c["Khoa"]
                        }).ToList();

                        return Json(new { Khoa = "Khóa đã tồn tại" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { Khoa = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetProjectList()
        {
            try
            {
                using (var cnn = new SqlConnection(cnnString))
                {
                    using (var cmd = new SqlCommand("sp_LayDanhSachDuAn", cnn))
                    {
                        cnn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TruongDuAnID", Session["UserID"].ToString());
                        var adpt = new SqlDataAdapter(cmd);
                        var tb = new DataTable();
                        adpt.Fill(tb);
                        string t = JsonConvert.SerializeObject(tb);
                        return Json(new { result = t, msg = "OK" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = -1, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetUserList()
        {
            try
            {
                using (var cnn = new SqlConnection(cnnString))
                {
                    using (var cmd = new SqlCommand("sp_LayDanhSachTaiKhoan", cnn))
                    {
                        cnn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TruongDuAnID", Session["UserID"].ToString());
                        var adpt = new SqlDataAdapter(cmd);
                        var tb = new DataTable();
                        adpt.Fill(tb);
                        string t = JsonConvert.SerializeObject(tb);
                        return Json(new { result = t, msg = "OK" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = -1, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetProjectDetail(int duAnId)
        {
            try
            {
                using (var cnn = new SqlConnection(cnnString))
                {
                    using (var cmd = new SqlCommand("sp_LayChiTietDuAn", cnn))
                    {
                        cnn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@duAnId", duAnId);
                        var adpt = new SqlDataAdapter(cmd);
                        var tb = new DataTable();
                        adpt.Fill(tb);
                        string t = JsonConvert.SerializeObject(tb);
                        return Json(new { result = t, msg = "OK" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = -1, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult CheckProjectName(string name)
        {
            try
            {
                using (var cnn = new SqlConnection(cnnString))
                {
                    using (var cmd = new SqlCommand("sp_KiemTraTrungTenDuAn", cnn))
                    {
                        cnn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TenDuAn", name);
                        cmd.Parameters.AddWithValue("@TruongDuAnID", Session["UserID"].ToString());
                        var adpt = new SqlDataAdapter(cmd);
                        var tb = new DataTable();
                        adpt.Fill(tb);
                        bool check = tb.Rows.Count > 0 ? false : true;
                        return Json(new { TenDuAn = check }, JsonRequestBehavior.AllowGet);

                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = -1, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
