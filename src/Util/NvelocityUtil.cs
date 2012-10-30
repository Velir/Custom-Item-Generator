using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using CustomItemGenerator.Settings;

namespace CustomItemGenerator.Util
{
	public class NvelocityUtil
	{
		public static string GetTemplateFolderPath()
		{
			return new CustomItemSettings(HttpContext.Current).NvelocityTemplatePath;
		}
	}
}
