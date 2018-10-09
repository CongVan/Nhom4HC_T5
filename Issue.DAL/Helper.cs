using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
namespace Issue.DAL
{
	public static class Helper
	{
		public static List<T> DataTableToList<T>(this DataTable table) where T : class, new()
		{
			List<T> result;
			try
			{
				List<T> list = new List<T>();
				foreach (DataRow current in table.AsEnumerable())
				{
					T t = Activator.CreateInstance<T>();
					PropertyInfo[] properties = t.GetType().GetProperties();
					int i = 0;
					while (i < properties.Length)
					{
						PropertyInfo propertyInfo = properties[i];
						try
						{
							PropertyInfo property = t.GetType().GetProperty(propertyInfo.Name);
							property.SetValue(t, Convert.ChangeType(current[propertyInfo.Name], property.PropertyType), null);
						}
						catch
						{
						}
						IL_96:
						i++;
						continue;
						goto IL_96;
					}
					list.Add(t);
				}
				result = list;
			}
			catch
			{
				result = null;
			}
			return result;
		}
	}
}
