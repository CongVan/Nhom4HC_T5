using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Member.Areas.Member.Controllers
{
    public class MemberController : Controller
    {
        static string cnnString = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
        //
        // GET: /Member/Member/
        public ActionResult MemberOfProject(int id)
        {
            return View(id);
        }
        public JsonResult GetMemberOfProject(int id)
        {
            try
            {
                using (var cnn = new SqlConnection(cnnString))
                {
                    using (var cmd = new SqlCommand("sp_LayDanhSachThanhVienDuAn", cnn))
                    {
                        cnn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", id);
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
        [HttpGet]
        public JsonResult GetNameOfProject(int id)
        {
            try
            {
                using (var cnn = new SqlConnection(cnnString))
                {
                    using (var cmd = new SqlCommand("sp_LayTenDuAn", cnn))
                    {
                        cnn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", id);
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
        public JsonResult GetUserList(int id)
        {
            try
            {
                using (var cnn = new SqlConnection(cnnString))
                {
                    using (var cmd = new SqlCommand("sp_LayDanhSachThanhVien", cnn))
                    {
                        cnn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", id);
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
        [HttpPost]
        public ActionResult AddProject(int id)
        {
            try
            {
                using (var cnn = new SqlConnection(cnnString))
                {
                    using (var cmd = new SqlCommand("sp_ThemThanhVienDuAn", cnn))
                    {
                        cnn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TaiKhoanID", Request["ThanhVienID"]);
                        cmd.Parameters.AddWithValue("@DuAnID", id);
                        cmd.Parameters.AddWithValue("@NguoiThemID", Session["UserID"]);
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
        public ActionResult DeleteProject(int id)
        {
            try
            {
                using (var cnn = new SqlConnection(cnnString))
                {
                    using (var cmd = new SqlCommand("sp_XoaThanhVienDuAn", cnn))
                    {
                        cnn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", id);
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
    }
}
