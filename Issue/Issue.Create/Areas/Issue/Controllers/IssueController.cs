using Issue.Create.Areas.Issue.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Issue.Create.Areas.Issue.Controllers
{
    public class IssueController : Controller
    {
        //
        // GET: /Issue/Issue/

        public ActionResult Create()
        {
            if (Request.QueryString["VanDeID"] != null)
            {
                ViewBag.VanDeID = Request.QueryString["VanDeID"];
                ViewBag.TextButton = "Cập nhật";
            }
            else
            {
                ViewBag.VanDeID = 0;
                ViewBag.TextButton = "Thêm mới";
            }

            return View();
        }

        public JsonResult LayDanhSachLoaiVanDe()
        {
            var result = IssueTypeModel.LoaiVanDe_LayDanhSach();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LayThongTinVanDe(int VanDeID)
        {
            var result = IssueModel.LayThongTinVanDe(VanDeID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LayDanhSachTaiKhoan()
        {
            var result = IssueModel.LayDanhSachTaiKhoan();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LayDanhSachDuAn()
        {
            var result = IssueModel.LayDanhSachDuAn();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CapNhatVanDe()
        {
            IssueModel IM = new IssueModel();
            IM.VanDeID = Int32.Parse(Request.Form["VanDeID"].ToString());
            IM.DuAnID = Int32.Parse(Request.Form["DuAnID"].ToString());
            IM.LoaiVanDeID = Int32.Parse(Request.Form["LoaiVanDe"].ToString());
            IM.TenVanDe = Request.Form["TenVanDe"].ToString();
            IM.MoTa = Request.Form["MoTa"].ToString();
            IM.TrangThai = Int32.Parse(Request.Form["TrangThai"].ToString());
            IM.NguoiThucHien = Request.Form["NguoiThucHien"].ToString();
            IM.NgayBatDau = Request.Form["NgayBatDau"].ToString();
            IM.NgayKetThuc = Request.Form["NgayKetThuc"].ToString();
            IM.SoGioDuKien = Int32.Parse(Request.Form["SoGioDuKien"].ToString());
            IM.SoGioThucTe = Int32.Parse(Request.Form["SoGioThucTe"].ToString());
            IM.NguoiCapNhat = Session["UserName"].ToString();

            ResData res = IssueModel.CapNhatVanDe(IM);

            if(res.ResultID == 1)
            {
                SendMail(res.ResultVanDeID);
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        private void SendMail(int VanDeID)
        {
            string result = "";
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                DataTable dt = IssueModel.LayThongTinChiTietVanDe(VanDeID);
                if(dt.Rows.Count > 0)
                {
                    dic["VanDeID"] = dt.Rows[0]["VanDeID"].ToString();
                    dic["TenDuAn"] = dt.Rows[0]["TenDuAn"].ToString();
                    dic["TenVanDe"] = dt.Rows[0]["TenVanDe"].ToString();
                    dic["MoTa"] = dt.Rows[0]["MoTa"].ToString();
                    dic["TenDangNhap"] = dt.Rows[0]["TenDangNhap"].ToString();
                    dic["NgayBatDau"] = dt.Rows[0]["NgayBatDau"].ToString();
                    dic["NgayKetThuc"] = dt.Rows[0]["NgayKetThuc"].ToString();
                    dic["SoGioDuKien"] = dt.Rows[0]["SoGioDuKien"].ToString();
                    dic["SoGioThucTe"] = dt.Rows[0]["SoGioThucTe"].ToString();
                    dic["NguoiTao"] = dt.Rows[0]["NguoiTao"].ToString();
                    dic["TrangThai"] = dt.Rows[0]["TrangThai"].ToString();
                    dic["Email"] = dt.Rows[0]["Email"].ToString();
                }

                if (!String.IsNullOrEmpty(dic["Email"].ToString()))
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
                    mail.To.Add(dic["Email"].ToString());
                    mail.Subject = "["+ dic["TenDuAn"].ToString() + "] - " + dic["TenVanDe"].ToString();
                    mail.IsBodyHtml = true;
                    mail.Body = "<p>Dear " + dic["Email"].ToString().Split('@')[0] + "</p>" +
                                "<p>"+ dic["NguoiTao"].ToString() + " đã phân công cho bạn thực hiện vấn đề sau: <a href=\"http://bug.muteso.com/Issue/Create?VanDeID="+ dic["VanDeID"].ToString() + "\">Xem tại đây</a></p>" +
                                "<ul> " +
                                "    <li> Người tạo: "+ dic["NguoiTao"].ToString() +" </li> " +
                                "    <li> Trạng thái: " + dic["TrangThai"].ToString() + " </li> " +
                                "    <li> Người thực hiện: " + dic["TenDangNhap"].ToString() + " </li> " +
                                "    <li> Ngày bắt đầu: " + dic["NgayBatDau"].ToString() + " </li> " +
                                "    <li> Ngày kết thúc: " + dic["NgayKetThuc"].ToString() + " </li> " +
                                "    <li> Số giờ dự kiến: " + dic["SoGioDuKien"].ToString() + " </li> " +                                
                                "</ul> " +
                                "<p>"+ dic["MoTa"].ToString() + "</p>";

                    SmtpServer.Send(mail);
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            //return result;            
        }

    }
}