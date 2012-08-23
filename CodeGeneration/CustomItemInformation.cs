using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using CustomItemGenerator.Interfaces;
using CustomItemGenerator.Providers;
using CustomItemGenerator.Settings;
using CustomItemGenerator.Util;
using Sitecore.Data.Items;

namespace CustomItemGenerator.CodeGeneration
{
	public class CustomItemInformation 
	{
		public string ClassName { get; private set; }

		public string BaseNamespace { get; private set; }
		public string FullNameSpace { get; private set; }


		public string BaseFileRoot { get; private set; }
		
		public TemplateItem Template { get; private set; }
		
		public List<string> Usings { get; private set; }
		public List<BaseTemplateInformation> BaseTemplates { get; private set; }
		public List<FieldInformation> Fields { get; private set; }

		public ICustomItemFolderPathProvider FolderPathProvider { get; private set; }
		public ICustomItemNamespaceProvider NamespaceProvider { get; private set; }

		public CustomItemInformation(TemplateItem template, string baseNamespace, string baseFileRoot)
		{
			CustomItemSettings settings = new CustomItemSettings(HttpContext.Current);
			ICustomItemNamespaceProvider namespaceProvider = AssemblyUtil.GetNamespaceProvider(settings.NamespaceProvider);
			ICustomItemFolderPathProvider filePathProvider = AssemblyUtil.GetFilePathProvider(settings.FilepathProvider);
			GetItemInformation(template, baseNamespace, baseFileRoot, filePathProvider, namespaceProvider);
		}

		public CustomItemInformation(TemplateItem template, string baseNamespace, string baseFileRoot, ICustomItemFolderPathProvider folderPathProvider, ICustomItemNamespaceProvider namespaceProvider)
		{
			this.GetItemInformation(template, baseNamespace, baseFileRoot, folderPathProvider, namespaceProvider);
		}

		private void GetItemInformation(TemplateItem template, 
																		string baseNamespace, 
																		string baseFileRoot, 
																		ICustomItemFolderPathProvider folderPathProvider, 
																		ICustomItemNamespaceProvider namespaceProvider)
		{
			ClassName = CodeUtil.GetClassNameForTemplate(template);

			Template = template;

			BaseNamespace = baseNamespace;
			FullNameSpace = namespaceProvider.GetNamespace(Template, BaseNamespace);

			BaseFileRoot = baseFileRoot;

			//Get all of the direct basetemplates
			BaseTemplates = new List<BaseTemplateInformation>();
			foreach (TemplateItem basetemplate in template.BaseTemplates)
			{
				//Skip the standard template
				if (basetemplate.Name == "Standard template") continue;

				BaseTemplates.Add(new BaseTemplateInformation(basetemplate, namespaceProvider));
			}

			//Create all the needed using statements for the base templates
			Usings = new List<string>();
			foreach (BaseTemplateInformation baseTemplateInformation in BaseTemplates)
			{
				if (!Usings.Contains(baseTemplateInformation.UsingNameSpace))
				{
					Usings.Add(baseTemplateInformation.UsingNameSpace);
				}
			}

			Fields = TemplateUtil.GetTemplateFieldInformation(template);

			FolderPathProvider = folderPathProvider;
			NamespaceProvider = namespaceProvider;
		}
	}
}