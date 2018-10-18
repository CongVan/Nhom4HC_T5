using System.Web.Mvc;

namespace Project.Management.Areas.Project
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
                "Project_Management",
                "Project/Management/{action}/{id}",
                new { Controller = "Project", action = "Project", id = UrlParameter.Optional },
                namespaces: new[] { "Project.Management.Areas.Project.Controllers" }
            );
        }
    }
}
