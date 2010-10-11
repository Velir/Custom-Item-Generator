using CustomItemGenerator.Interfaces;
using CustomItemGenerator.Settings;
using CustomItemGenerator.Util;
using Sitecore.Data.Items;

namespace CustomItemGenerator.CodeGeneration
{
	public class BaseTemplateInformation 
	{
		public string PropertyName { get; private set; }
		public string ClassName { get; private set; }
		public string UsingNameSpace { get; private set; }

		public BaseTemplateInformation(TemplateItem template,ICustomItemNamespaceProvider namespaceProvider)
		{
			ClassName = CodeUtil.GetClassNameForTemplate(template);

			PropertyName = ClassName.Remove(ClassName.Length - 4);
			if(PropertyName.StartsWith("_"))
			{
				PropertyName = PropertyName.Substring(1);
			}

			CustomItemSettings settings = new CustomItemSettings();
			UsingNameSpace = namespaceProvider.GetNamespace(template, settings.BaseNamespace);
		}
	}
}
