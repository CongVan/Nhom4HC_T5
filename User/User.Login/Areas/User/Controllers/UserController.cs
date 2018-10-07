using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using User.Login.Areas.User.Models;
using User.Login.Areas.User.Services;


namespace User.Login.Areas.User.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/Login/

        public ActionResult Login()
        {
            if (Session["UserName"] == null)
            {
                return View();
            }
            else
            {
                return Redirect("~/");
            }
        }
        public ActionResult Logout()
        {
            Session["UserName"] = null;
            Session["UserID"] = null;
            return Redirect("~/User/Login/");
        }
        [HttpPost]
        public JsonResult LoginUser()
        {
            var model = Request.Form["usermodel"];
            var userModel = JsonConvert.DeserializeObject<UserModel>(model);
            userModel.MatKhau = CreateMD5(userModel.MatKhau);
            var service = new UserService();
            var result = service.DangNhapTaiKhoan(userModel);
            if (Int32.Parse(result) > 0)
            {
                Session["UserID"] = Int32.Parse(result);
                Session["UserName"] = userModel.TenDangNhap.ToString();
                if (userModel.NhoTaiKhoan == true)
                {
                    Session["RememberMe"] = "checked";
                    Session["RememberAccount"] = userModel.TenDangNhap;
                }
                else
                {
                    Session["RememberMe"] = "";
                    Session["RememberAccount"] = "";
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
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
