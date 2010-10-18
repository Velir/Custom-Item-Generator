using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CustomItemGenerator.Interfaces;
using CustomItemGenerator.Util;
using Sitecore.Data.Items;

namespace CustomItemGenerator.Providers
{
	/// <summary>
	/// Creates the namespace from a template's item path
	/// </summary>
	public class TemplatePathNameSpaceProvider : ICustomItemNamespaceProvider
	{

		public string GetNamespace(TemplateItem template, string baseNamespace)
		{
			string relativePath = TemplateUtil.GetRelativeTemplatePath(template);
			
			if(string.IsNullOrEmpty(relativePath)) return baseNamespace;

			relativePath = relativePath.Replace("/", ".");
			relativePath = CodeUtil.CleanStringOfIllegalCharacters(relativePath);

			return baseNamespace + "." + relativePath;
			
		}
	}
}
