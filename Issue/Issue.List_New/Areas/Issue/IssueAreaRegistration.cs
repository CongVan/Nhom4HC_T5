using System.Web.Mvc;

namespace Issue.List_New.Areas.Issue
{
    public class IssueAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Issue";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Issue_List",
                "Issue/List_New/{action}/{id}",
                new { Controller = "List", action = "List_New", id = UrlParameter.Optional }
                , namespaces: new[] { "Issue.List_New.Areas.Issue.Controllers" }
            );
        }
    }
}
