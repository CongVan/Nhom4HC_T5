using System.Web.Mvc;

namespace Project.Member.Areas.Member
{
    public class MemberAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Member";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Project_Member",
                "Project/Member/{action}/{id}",
                new { Controller = "Member", action = "MemberOfProject", id = UrlParameter.Optional }, 
                namespaces: new[] { "Project.Member.Areas.Member.Controllers" }
            );
        }
    }
}
