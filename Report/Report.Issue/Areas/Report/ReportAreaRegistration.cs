using System.Web.Mvc;

namespace Report.Issue.Areas.Report
{
    public class ReportAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Report";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Report_Issue",
                "Report/Issue/{action}/{id}",
                new { Controller = "Report", action = "Index", id = UrlParameter.Optional }
                , namespaces: new[] { "Report.Issue.Areas.Report.Controllers" }
            );
        }
    }
}
