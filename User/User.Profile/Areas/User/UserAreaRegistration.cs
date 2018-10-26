using System.Web.Mvc;

namespace User.Profile.Areas.User
{
    public class UserAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "User";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "User_Profile",
                "User/Profile/{action}/{id}",
                new { Controller = "User",action = "Profile", id = UrlParameter.Optional }
                , namespaces: new[] {"User.Profile.Areas.User.Controllers"}
            );
        }
    }
}
