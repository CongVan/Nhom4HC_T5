using System.Web.Mvc;

namespace User.Login.Areas.User
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
                "User_Login",
                "User/Login/{action}/{id}",
                new {Controller="User", action = "Login", id = UrlParameter.Optional },
                namespaces: new[] { "User.Login.Areas.User.Controllers" }
            );
            context.MapRoute(
                "User_Logout",
                "User/Logout/{action}/{id}",
                new { Controller = "User", action = "Logout", id = UrlParameter.Optional },
                namespaces: new[] { "User.Login.Areas.User.Controllers" }

            );
        }
    }
}
