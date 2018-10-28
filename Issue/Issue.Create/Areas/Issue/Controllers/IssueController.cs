using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Issue.Create.Areas.Issue.Controllers
{
    public class IssueController : Controller
    {
        //
        // GET: /Issue/Issue/
        string cnnString = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;

        public ActionResult Create()
        {
            ViewBag.ConnectionString = cnnString;
            return View();
        }

    }
}
