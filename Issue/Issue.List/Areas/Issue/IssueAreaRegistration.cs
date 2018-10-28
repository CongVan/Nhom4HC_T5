using System.Web.Mvc;

namespace Issue.List.Areas.Issue
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
                "Issue/List/{action}/{id}",
                new { Controller = "List", action = "List", id = UrlParameter.Optional }
                , namespaces: new[] { "Issue.List.Areas.Issue.Controllers" }
            );
        }
    }
}