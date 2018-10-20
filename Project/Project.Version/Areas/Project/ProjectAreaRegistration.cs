using System.Web.Mvc;

namespace Project.Version.Areas.Project
{
    public class ProjectAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Project";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Project_version",
                "Project/Version/{action}/{id}",
                new { Controller = "Project",action = "Version", id = UrlParameter.Optional }
                , namespaces: new[] {"Project.Version.Areas.Project.Controllers"}
            );
        }
    }
}
