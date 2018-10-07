using Newtonsoft.Json;
using SRVTextToImage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using User.Register.Areas.User.Models;
using User.Register.Areas.User.Services;

namespace User.Register.Areas.User.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/Register/

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public JsonResult RegisterUser()
        {
            var model = Request.Form["usermodel"];
            var userModel = JsonConvert.DeserializeObject<UserModel>(model);
            userModel.NgayTao = DateTime.Now;
            userModel.TinhTrang = 1;
            userModel.MatKhau = CreateMD5(userModel.MatKhau);
            userModel.LoaiTaiKhoanID = 11;//Edit after
            var service = new UserService();
            var result = service.DangKyTaiKhoan(userModel);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CheckUserName(string username)
        {
            var service = new UserService();
            var result = service.KiemTraTenDangNhap(username);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult CheckCaptchaValidate(string captchaCode)
        {
            var codeSession = this.Session["CaptChaImageText"].ToString();
            if (codeSession == captchaCode)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [OutputCache(NoStore=true,Duration=0,VaryByParam="None")]
        public FileResult GetCaptChaImage()
        {
            CaptchaRandomImage CI = new CaptchaRandomImage();
            this.Session["CaptChaImageText"] = CI.GetRandomString(5);
            CI.GenerateImage(this.Session["CaptChaImageText"].ToString(), 300, 75, Color.DarkGray, Color.White);
            MemoryStream stream = new MemoryStream();
            CI.Image.Save(stream, ImageFormat.Png);
            stream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(stream, "image/png");
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

    }
}
