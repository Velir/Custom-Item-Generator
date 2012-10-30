using System;
using System.Configuration;
using System.Web;
using CustomItemGenerator.CodeGeneration;
using CustomItemGenerator.Interfaces;
using CustomItemGenerator.Providers;
using CustomItemGenerator.Settings;
using CustomItemGenerator.Util;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Web;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Pages;
using Sitecore.Web.UI.Sheer;

namespace CustomItemGenerator.SitecoreApp
{
	public class CustomItemCodeBeside : DialogForm
	{
		protected Database masterDb = Factory.GetDatabase("master");
		protected Edit CustomItemNamespace;
		protected Edit CustomItemFilePath;
		private TemplateItem template;
		
		private Checkbox GenerateBaseFile;
		private Checkbox GenerateInstanceFile;
		private Checkbox GenerateInterfaceFile;
		private Checkbox GenerateStaticFile;
		
		/// <summary>
		/// Gets the current content GUID, this will be empty if we are not 
		///   viewing the comments for a specific piece of content.
		/// </summary>
		/// <returns></returns>
		protected string GetCurrentContentGuid()
		{
			string contentGuid = WebUtil.GetQueryString("id");
			return !string.IsNullOrEmpty(contentGuid) ? contentGuid : string.Empty;
		}

		protected override void OnLoad(EventArgs args)
		{
			base.OnLoad(args);
			string currentItemId = GetCurrentContentGuid();
			template = masterDb.GetItem(currentItemId);
			if(template == null)
			{
				SheerResponse.Alert("You must select a template to continue.", new string[0]);
			}
			else
			{
				if (!Context.ClientPage.IsEvent)
				{
					CustomItemSettings settings = new CustomItemSettings(HttpContext.Current);

					string defaultNamespace = settings.BaseNamespace;
					string defaultFilePath = settings.BaseFileOutputPath;

					if(!string.IsNullOrEmpty(defaultNamespace))
					{
						CustomItemNamespace.Value = defaultNamespace;
					}

					if (!string.IsNullOrEmpty(defaultFilePath))
					{
						CustomItemFilePath.Value = defaultFilePath;
					}
					else
					{
						CustomItemFilePath.Value = "[ENTER CUSTOM ITEM ROOT FILE PATH]";
					}
				}
			}
		}

		protected static string GetFilePathForFolders(Item item)
		{
			//If the parent is the template root, we're done
			if (item.Parent.Paths.Path == "/sitecore/templates")
				return string.Empty;

			return GetFilePathForFolders(item.Parent) + "\\" + item.Name;
		}

		protected override void OnOK(object sender, EventArgs args)
		{
			CustomItemSettings settings = new CustomItemSettings(HttpContext.Current);

			ICustomItemNamespaceProvider namespaceProvider = AssemblyUtil.GetNamespaceProvider(settings.NamespaceProvider);
			ICustomItemFolderPathProvider filePathProvider = AssemblyUtil.GetFilePathProvider(settings.FilepathProvider);

			CustomItemInformation customItemInformation = new CustomItemInformation(template, CustomItemNamespace.Value,
																																							CustomItemFilePath.Value, filePathProvider, namespaceProvider);
			CodeGenerator codeGenerator = new CodeGenerator(customItemInformation,
						GenerateBaseFile.Checked, GenerateInstanceFile.Checked, GenerateInterfaceFile.Checked, GenerateStaticFile.Checked);
			codeGenerator.GenerateCode();

			SheerResponse.Alert(codeGenerator.GenerationMessage, new string[0]);

			base.OnOK(sender, args);
		}
	}
}