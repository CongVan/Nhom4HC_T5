using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using User.ChangePass.Areas.User.Models;
using User.ChangePass.Areas.User.Services;

namespace User.ChangePass.Areas.User.Controllers
{
    public class ChangePassController : Controller
    {
        //
        // GET: /User/ChangePass/

        public ActionResult ChangePass()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ChangePassUser()
        {
            var model =  Request.Form["usermodel"];
            var userModel = JsonConvert.DeserializeObject<UserModel>(model);
            userModel.MatKhau = CreateMD5(userModel.MatKhau);
            userModel.MatKhauMoi = CreateMD5(userModel.MatKhauMoi);
            var service = new UserServices();
            var result = service.DoiMatKhau(userModel);
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
