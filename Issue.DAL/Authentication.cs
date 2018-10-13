using System;
using System.Web;
namespace Libraries
{
	public class Authentication
	{
		private string urlLogin = "\\user\\login";
		public bool isAuthentication = false;
		public Authentication(HttpContextBase httpContext)
		{
			try
			{
				if (httpContext.Session["UserName"] != null)
				{
					this.isAuthentication = true;
				}
				else
				{
					if (httpContext.Session["UserName"] == null)
					{
						httpContext.Response.Redirect(this.urlLogin);
					}
					this.isAuthentication = true;
				}
			}
			catch (Exception ex)
			{
				WriteLog.writeToLogFile(ex);
				this.isAuthentication = false;
			}
		}
	}
}
