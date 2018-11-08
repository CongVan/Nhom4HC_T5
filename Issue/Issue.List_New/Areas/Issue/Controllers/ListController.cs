using Issue.List.Areas.Issue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Issue.List_New.Areas.Issue.Controllers
{
    public class ListController : Controller
    {
        // GET: Issue/List
        public ActionResult List_New()
        {
            return View();
        }

        public JsonResult GetList()
        {
            string TenDangNhap = Session["UserName"].ToString();
            List<IssueModel> lst = IssueModel.GetList(TenDangNhap);

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CapNhatTinhTrang(int VanDeID)
        {
            ResData res = new ResData();

            res = IssueModel.CapNhatTinhTrang(VanDeID, Session["Username"].ToString());

            return Json(res, JsonRequestBehavior.AllowGet); ;
        }
    }
}
