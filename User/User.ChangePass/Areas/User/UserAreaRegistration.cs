using System.Web.Mvc;

namespace User.ChangePass.Areas.User
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
                "User_ChangePass",
                "User/ChangePass/{action}/{id}",
                new { Controller = "ChangePass", action = "ChangePass", id = UrlParameter.Optional },
                namespaces: new[] { "User.ChangePass.Areas.User.Controllers" }
            );
        }
    }
}
