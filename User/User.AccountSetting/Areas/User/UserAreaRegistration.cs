using System.Web.Mvc;

namespace User.AccountSetting.Areas.User
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
                "User_AccountSetting",
                "User/AccountSetting/{action}/{id}",
                new { Controller = "User", action = "AccountSetting", id = UrlParameter.Optional },
                namespaces: new[] { "User.AccountSetting.Areas.User.Controllers" }
            );
        }
    }
}
