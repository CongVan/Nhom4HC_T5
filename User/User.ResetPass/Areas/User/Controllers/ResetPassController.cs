using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using User.ResetPass.Areas.User.Models;

namespace User.ResetPass.Areas.User.Controllers
{
    public class ResetPassController : Controller
    {
        //
        // GET: /User/ResetPass/

        public ActionResult ResetPass()
        {
            return View();
        }

        public JsonResult LayLaiMatKhau(string Email)
        {
            string new_MatKhau = RandomPassword(8);
            string new_MatKhau_MD5 = CreateMD5(new_MatKhau);
            var result = UserModel.ResetMatKhau(Email, new_MatKhau_MD5);

            if (result.ResultID == 1)
            {
                GuiEmail(Email, new_MatKhau);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        private void GuiEmail(string Email, string MatKhau)
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
                mail.Subject = "Mật khẩu của bạn đã được reset";
                mail.IsBodyHtml = true;
                mail.Body = "<p>Dear "+ Email.Split('@')[0] + "</p>" +
                            "<p>Bạn đã yêu cầu lấy lại mật khẩu</p>" +
                            "<p>Mật khẩu của bạn đã được hệ thống thay đổi thành: <strong>" +MatKhau+"</strong>.</p>" +
                            "<br><p>Thanks!</p>";                         

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                result = ex.Message;                
            }
            //return result;
        }
        public string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public string RandomPassword(int length)
        {
            Random rdom = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[rdom.Next(s.Length)]).ToArray());
        }

    }
}
