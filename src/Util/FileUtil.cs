using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;

namespace CustomItemGenerator.Util
{
	public class FileUtil
	{
		public static string GetClassFilePath(string className, string folderPath)
		{
			return folderPath + "\\" + className + ".base.cs";
		}
	}
}
