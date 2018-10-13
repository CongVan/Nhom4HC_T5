using System;
using System.Data;
using System.Text;
using System.Web;
namespace Issue.DAL
{
	public class Lib
	{
		public static string Paging(int Total, int CurrPage, int PageSize, int RecodPerPage, string RawUrl)
		{
			if (RecodPerPage < 1)
			{
				RecodPerPage = 1;
			}
			if (!RawUrl.Contains("?"))
			{
				RawUrl += "?";
			}
			if (RawUrl.IndexOf("CurrPage") != -1)
			{
				RawUrl = RawUrl.Substring(0, RawUrl.IndexOf("CurrPage") - 1);
			}
			int num = 0;
			int num2;
			if (Total % RecodPerPage > 0)
			{
				num2 = Total / RecodPerPage + 1;
			}
			else
			{
				num2 = Total / RecodPerPage;
			}
			string text = "<div class=pagination>";
			if (CurrPage <= num2)
			{
				int num3;
				if (CurrPage == 1)
				{
					if (num == 1)
					{
						text += "Page";
					}
					num = PageSize;
					if (num > num2)
					{
						num = num2;
					}
					num3 = 1;
				}
				else
				{
					if (!RawUrl.Contains("?"))
					{
						RawUrl += "?";
					}
					text = string.Format("{0}<a href=" + RawUrl + "&CurrPage=1><B> First </B></a>", text);
					text = string.Format("{0}<a href=" + RawUrl + "&CurrPage={1}><B> Prev </B></a>", text, CurrPage - 1);
					if (num2 - CurrPage < PageSize / 2)
					{
						num3 = num2 - PageSize + 1;
						if (num3 <= 0)
						{
							num3 = 1;
						}
						num = num2;
					}
					else if (CurrPage - PageSize / 2 == 0)
					{
						num3 = 1;
						num = CurrPage + PageSize / 2 + 1;
						if (num2 < num)
						{
							num = num2;
						}
					}
					else
					{
						num3 = CurrPage - PageSize / 2;
						if (num3 <= 0)
						{
							num3 = 1;
						}
						num = CurrPage + PageSize / 2;
						if (num2 < num)
						{
							num = num2;
						}
						else if (num < PageSize)
						{
							num = PageSize;
						}
					}
				}
				for (int i = num3; i <= num; i++)
				{
					if (i == CurrPage)
					{
						text = string.Concat(new object[]
						{
							text,
							"<span class=current>",
							i,
							"</span>"
						});
					}
					else
					{
						text = string.Concat(new object[]
						{
							text,
							"<a href=",
							RawUrl,
							"&CurrPage=",
							i,
							">",
							i,
							"</a> "
						});
					}
				}
				if (CurrPage < num2)
				{
					text = string.Concat(new object[]
					{
						text,
						"<a href=",
						RawUrl,
						"&CurrPage=",
						CurrPage + 1,
						"><B> Next </B></a>"
					});
					text = string.Concat(new object[]
					{
						text,
						"<a href=",
						RawUrl,
						"&CurrPage=",
						num2,
						"><B> Last </B></a>"
					});
				}
			}
			return text + "</div>";
		}
		public static string DatatableToJson(DataTable dtTemp)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (DataRow dataRow in dtTemp.Rows)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append("{");
				StringBuilder stringBuilder2 = new StringBuilder();
				foreach (DataColumn dataColumn in dtTemp.Columns)
				{
					string columnName = dataColumn.ColumnName;
					string arg = dataRow[columnName].ToString();
					if (stringBuilder2.Length != 0)
					{
						stringBuilder2.Append(",");
					}
					stringBuilder2.Append(string.Format("{0}:\"{1}\"", columnName, arg));
				}
				stringBuilder.Append(stringBuilder2.ToString());
				stringBuilder.Append("}");
			}
			stringBuilder.Insert(0, "[");
			stringBuilder.Append("]");
			return stringBuilder.ToString();
		}
		public static string GetClientIP()
		{
			string text = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
			try
			{
				if (string.IsNullOrEmpty(text))
				{
					text = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
				}
				else
				{
					text = text.Split(new char[]
					{
						','
					})[0];
				}
			}
			catch (Exception)
			{
				text = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
			}
			return text;
		}
	}
}
