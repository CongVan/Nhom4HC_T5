using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using User.AccountSetting.Areas.User.Models;
using User.AccountSetting.Areas.User.Service;
namespace User.AccountSetting.Areas.User.Controllers
{
    public class UserController : Controller
    {
        //
        // Post: /User/AccountSetting
        
        public ActionResult AccountSetting()
        {
            
            if (System.Web.HttpContext.Current.Session["UserID"] != null)
            {
                int uID = (int)System.Web.HttpContext.Current.Session["UserID"];
                var service = new UserService();
                UserModel result = service.GetInfoUser(uID);
                ViewData["Mes"] = result;
                return View();
            }
            else
            {
                return Redirect("~/");
            }
        }
        [HttpPost]
        public JsonResult AccountSettingUser()
        {
            var model = Request.Form["usermodel"];
            var userModel = JsonConvert.DeserializeObject<UserModel>(model);
            //userModel.NgayTao = DateTime.Now;
            //userModel.TinhTrang = 1;
            userModel.TaiKhoanID = (int)System.Web.HttpContext.Current.Session["UserID"];
            var service = new UserService();
            var result = service.CapNhatTaiKhoan(userModel);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
