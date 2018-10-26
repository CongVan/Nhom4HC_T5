using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using User.Profile.Areas.User.Models;
using User.Profile.Areas.User.Service;
namespace User.Profile.Areas.User.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/Profile
        
        public ActionResult Profile()
        {

            if (System.Web.HttpContext.Current.Session["UserID"] != null)
            {
                int uID = (int)System.Web.HttpContext.Current.Session["UserID"];
                var service = new UserService();
                UserModel result = service.XemThongTinTaiKhoan(uID);
                ViewData["Message"] = result;
                return View();
            }
            else
            {
                return Redirect("~/");
            }
        }

    }
}
