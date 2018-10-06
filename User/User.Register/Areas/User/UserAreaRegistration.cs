using System.Web.Mvc;

namespace User.Register.Areas.User
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
                "User_Register",
                "User/Register/{action}/{id}",
                new { Controller="User",action = "Register", id = UrlParameter.Optional }
                , namespaces: new[] { "User.Register.Areas.User.Controllers" }
            );
        }
    }
}
