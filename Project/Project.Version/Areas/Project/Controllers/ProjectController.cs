using Newtonsoft.Json;
using Project.Version.Areas.Project.Models;
using Project.Version.Areas.Project.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Version.Areas.Project.Controllers
{
    public class ProjectController : Controller
    {
        //
        // GET: /Project/Project/
        
        public ActionResult Index(string id)
        {
            ViewBag.IdProject = id;
            var service = new VersionService();
            var projectName = service.GetNameProject(Convert.ToInt32(id));
            if (!string.IsNullOrEmpty(projectName))
                ViewBag.ProjectName = projectName;
            return View();
        }

        [HttpPost]
        public ActionResult InsUpdVersion()
        {
            var model = Request.Form["model"];
            var versionModel = JsonConvert.DeserializeObject<VersionModel>(model);
            if (versionModel.Id != null && versionModel.Id > 0)
            {
                versionModel.NgayCapNhat = DateTime.Now;
            }
            else
            {
                versionModel.NguoiTao = Session["UserID"] != null ? Int32.Parse(Session["UserID"].ToString()) : 0;
                versionModel.NgayTao = DateTime.Now;
            }
            var service = new VersionService();
            var result = service.InsUpdVersion(versionModel);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllVersion()
        {
            var service = new VersionService();
            var result = service.GetAllVersion(1);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
