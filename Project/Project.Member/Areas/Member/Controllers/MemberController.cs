using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
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
            if (Session["UserID"]==null)
            {
                return Redirect("~/User/Login/");
            }
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
        public JsonResult RollOfProject()
        {
            try
            {
                using (var cnn = new SqlConnection(cnnString))
                {
                    using (var cmd = new SqlCommand("sp_LayDanhSachChucVu", cnn))
                    {
                        cnn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
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
        public JsonResult LeaderOfProject(int id)
        {
            try
            {
                using (var cnn = new SqlConnection(cnnString))
                {
                    using (var cmd = new SqlCommand("sp_LayTruongDuAn", cnn))
                    {
                        cnn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", id);
                        cmd.Parameters.AddWithValue("@IDCheck", Session["UserID"]);
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
                        cmd.Parameters.AddWithValue("@Roll", Request["RollID"]);
                        int c = cmd.ExecuteNonQuery();
                        using (var cnn2 = new SqlConnection(cnnString))
                        {
                            using (var cmd2 = new SqlCommand("sp_LayDanhSachEmailTVDA", cnn))
                            {
                                cnn2.Open();
                                cmd2.CommandType = CommandType.StoredProcedure;
                                cmd2.Parameters.AddWithValue("@ID", id);
                                var adpt = new SqlDataAdapter(cmd2);
                                var tb = new DataTable();
                                string thanhVienMoiTDN="";
                                string thanhVienMoiHoTen = "";
                                adpt.Fill(tb);
                                if (tb != null && tb.Rows.Count > 0)
                                {
                                    for (int i = 0; i < tb.Rows.Count; i++)
                                    {
                                        int id1 = Int32.Parse(tb.Rows[i]["TaiKhoanID"].ToString());
                                        int id2 = Int32.Parse(Request["ThanhVienID"].ToString());
                                        if (id1 == id2)
                                        {
                                            string email = tb.Rows[i]["Email"].ToString();
                                            string name = tb.Rows[i]["TenDuAn"].ToString();
                                            thanhVienMoiTDN = tb.Rows[i]["TenDangNhap"].ToString();
                                            thanhVienMoiHoTen = tb.Rows[i]["HoTen"].ToString();
                                            GuiEmail1(email, name);
                                        }
                                    }
                                    for (int i = 0; i < tb.Rows.Count; i++)
                                    {
                                        int id1 = Int32.Parse(tb.Rows[i]["TaiKhoanID"].ToString());
                                        int id2 = Int32.Parse(Request["ThanhVienID"].ToString());
                                        if (id1 != id2)
                                        {
                                            string email = tb.Rows[i]["Email"].ToString();
                                            string name = tb.Rows[i]["TenDuAn"].ToString();
                                            GuiEmail2(email, name, thanhVienMoiTDN, thanhVienMoiHoTen);
                                        }
                                    }
                                    
                                }

                            }
                        }
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
        private void GuiEmail1(string Email, string TenDuAn)
        {
            string result = "";
            try
            {
                SmtpClient SmtpServer = new SmtpClient();
                SmtpServer.Host = "smtp.gmail.com";
                SmtpServer.Port = 587;
                SmtpServer.EnableSsl = true;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("17hcbquanlyloi@gmail.com", "Abc@12345");

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("17hcbquanlyloi@gmail.com");
                mail.To.Add(Email);
                mail.Subject = "Thông báo thay đổi thành viên dự án";
                mail.IsBodyHtml = true;
                mail.Body = "<p>Dear " + Email.Split('@')[0] + "</p>" +
                            "<p>Bạn đã được thêm vào dự án: "+TenDuAn+"</p>" +
                            "<br><p>Thanks!</p>";

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            //return result;
        }
        private void GuiEmail2(string Email, string TenDuAn, string TenDangNhap, string HoTen)
        {
            string result = "";
            try
            {
                SmtpClient SmtpServer = new SmtpClient();
                SmtpServer.Host = "smtp.gmail.com";
                SmtpServer.Port = 587;
                SmtpServer.EnableSsl = true;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("17hcbquanlyloi@gmail.com", "Abc@12345");

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("17hcbquanlyloi@gmail.com");
                mail.To.Add(Email);
                mail.Subject = "Thông báo thay đổi thành viên dự án";
                mail.IsBodyHtml = true;
                mail.Body = "<p>Dear " + Email.Split('@')[0] + "</p>" +
                            "<p>Dự án: " + TenDuAn + " đã thêm thành viên mới </p>" +
                            "<p>Tên đăng nhập: "+ TenDangNhap + "</p>" + 
                            "<p>Họ tên:" + HoTen +"</p>" +
                            "<br><p>Thanks!</p>";

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            //return result;
        }
    }
}
