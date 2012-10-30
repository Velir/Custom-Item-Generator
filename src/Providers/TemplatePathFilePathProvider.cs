using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CustomItemGenerator.Interfaces;
using CustomItemGenerator.Util;
using Sitecore.Data.Items;

namespace CustomItemGenerator.Providers
{
	public class TemplatePathFilePathProvider : ICustomItemFolderPathProvider
	{
		

		public string GetFolderPath(TemplateItem template,string baseFilePath)
		{
			string relativeTemplatePath = TemplateUtil.GetRelativeTemplatePath(template).Replace("/", "\\");
			relativeTemplatePath = CodeUtil.CleanStringOfIllegalCharacters(relativeTemplatePath);

			if(!baseFilePath.EndsWith("\\"))
			{
				baseFilePath += "\\";
			}

			return baseFilePath + relativeTemplatePath;
		}
	}
}
