using System;
using System.IO;
using System.Web;
public static class WriteLog
{
	public static void writeToLogFile(Exception ex)
	{
		try
		{
			string str = ex.Message + " ; " + ex.ToString();
			string value = string.Empty;
			if (!Directory.Exists(HttpContext.Current.Server.MapPath("//Log//")))
			{
				Directory.CreateDirectory(HttpContext.Current.Server.MapPath("//Log//"));
			}
			string path = HttpContext.Current.Server.MapPath("//Log//" + string.Format("{0}-{1}-{2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + ".txt");
			value = string.Format("{0}: {1}", DateTime.Now, HttpContext.Current.Request.Path + "      " + str);
			StreamWriter streamWriter;
			if (!File.Exists(path))
			{
				streamWriter = new StreamWriter(path);
			}
			else
			{
				streamWriter = File.AppendText(path);
			}
			streamWriter.WriteLine(value);
			streamWriter.WriteLine();
			streamWriter.Close();
		}
		catch (Exception)
		{
		}
	}
}
