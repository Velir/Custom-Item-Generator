using System.Collections.Generic;
using System.Web;
using System.Xml;
using Sitecore.Configuration;
using Sitecore.Data.Items;

namespace CustomItemGenerator.Settings
{
	public class CustomItemSettings
	{
		public string BaseNamespace { get; private set; }
		public string BaseFileOutputPath { get; private set; }
		public string NvelocityTemplatePath { get; private set; }
		public string NamespaceProvider { get; private set; }
		public string FilepathProvider { get; private set; }
		public List<FieldMapping> FieldMappings { get; private set; }

		public CustomItemSettings()
		{
			XmlNode n = Factory.GetConfigNode("customItem");

			BaseNamespace = string.Empty;
			BaseFileOutputPath = string.Empty;
			NvelocityTemplatePath = string.Empty;
			NamespaceProvider = string.Empty;
			FilepathProvider = string.Empty;
			FieldMappings = new List<FieldMapping>();

			//Set the base path to the nvelocity templates
			string basePath = HttpContext.Current.Server.MapPath("/");
			NvelocityTemplatePath = basePath + @"\sitecore modules\Shell\CustomItemGenerator\Nvelocity Templates";

			foreach (XmlNode childNode in n.ChildNodes)
			{
				switch (childNode.Name)
				{
					case "Base.Namespace":
						BaseNamespace = childNode.InnerText;
						break;

					case "Base.FileOutputPath":
						BaseFileOutputPath = childNode.InnerText;
						break;

					case "Provider.Namespace":
						NamespaceProvider = childNode.InnerText;
						break;

					case "Provider.Filepath":
						FilepathProvider = childNode.InnerText;
						break;

					case "FieldMappings":

						foreach (XmlNode fieldMappingNode in childNode.ChildNodes)
						{
							if (fieldMappingNode != null)
							{
								FieldMappings.Add(new FieldMapping(fieldMappingNode));
							}
						}

						break;
				}
			}
		}
	}
}