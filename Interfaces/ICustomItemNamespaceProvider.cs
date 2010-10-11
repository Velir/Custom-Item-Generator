using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;

namespace CustomItemGenerator.Interfaces
{
	public interface ICustomItemNamespaceProvider
	{
		string GetNamespace(TemplateItem template, string baseNamespace);
	}
}
