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

		public CustomItemSettings(HttpContext httpContext)
			: this(httpContext.Server.MapPath("/"))
		{
		}

		public CustomItemSettings(string basePath)
		{
			string filename = basePath + @"\App_Config\Include\CustomItem.config";
			XmlDocument document = new XmlDocument();
			document.Load(filename);
			XmlNodeList elementsByTagName = document.GetElementsByTagName("customItem");

			BaseNamespace = string.Empty;
			BaseFileOutputPath = string.Empty;
			NvelocityTemplatePath = string.Empty;
			NamespaceProvider = string.Empty;
			FilepathProvider = string.Empty;
			FieldMappings = new List<FieldMapping>();

			NvelocityTemplatePath = basePath + @"\sitecore modules\Shell\CustomItemGenerator\Nvelocity Templates";

			foreach (XmlNode childNode in elementsByTagName[0].ChildNodes)
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