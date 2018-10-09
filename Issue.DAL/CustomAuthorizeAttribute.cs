using Libraries;
using System;
using System.Web;
using System.Web.Mvc;
public class CustomAuthorizeAttribute : AuthorizeAttribute
{
	protected override bool AuthorizeCore(HttpContextBase httpContext)
	{
		bool result;
		try
		{
			Authentication authentication = new Authentication(httpContext);
			if (authentication.isAuthentication)
			{
				string value = (string)httpContext.Session["UserName"];
				if (string.IsNullOrEmpty(value))
				{
					authentication.isAuthentication = false;
				}
			}
			result = authentication.isAuthentication;
		}
		catch (Exception ex)
		{
			WriteLog.writeToLogFile(ex);
			result = false;
		}
		return result;
	}
}
