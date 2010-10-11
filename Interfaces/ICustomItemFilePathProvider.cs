using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;

namespace CustomItemGenerator.Interfaces
{
	public interface ICustomItemFolderPathProvider
	{
		string GetFolderPath(TemplateItem template, string baseFilePath);
	}
}
