using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CustomItemGenerator.CodeGeneration;
using CustomItemGenerator.Interfaces;
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
	public class CustomItemFolderCodeBeside : DialogForm
	{
		protected Database masterDb = Factory.GetDatabase("master");
		protected Edit CustomItemNamespace;
		protected Edit CustomItemFilePath;
		protected Checklist TemplateList;
		private Item templateFolder;

		private Checkbox GenerateBaseFile;
		private Checkbox GenerateInstanceFile;
		private Checkbox GenerateInterfaceFile;
		private Checkbox GenerateStaticFile;

		/// <summary>
		/// Gets the current content GUID.
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
			templateFolder = masterDb.GetItem(currentItemId);
			if (templateFolder == null)
			{
				SheerResponse.Alert("You must select a template to continue.", new string[0]);
			}
			else
			{
				if (!Context.ClientPage.IsEvent)
				{
					CustomItemSettings settings = new CustomItemSettings(HttpContext.Current);

					//Set the default namespace text box
					string defaultNamespace = settings.BaseNamespace;
					if (!string.IsNullOrEmpty(defaultNamespace))
					{
						CustomItemNamespace.Value = defaultNamespace;
					}

					//Set the default file path text box
					string defaultFilePath = settings.BaseFileOutputPath;
					if (!string.IsNullOrEmpty(defaultFilePath))
					{
						CustomItemFilePath.Value = defaultFilePath;
					}
					else
					{
						CustomItemFilePath.Value = "Custom Item Root Path";
					}

					//Fill the list of templates with the templates 
					//that are descendent items of the selected folder
					FillTemplateList();
				}
			}
		}

		/// <summary>
		/// Fills the template list with applicable templates.
		/// </summary>
		protected void FillTemplateList()
		{
			TemplateList.Controls.Clear();

			List<Item> templateSubitems = TemplateUtil.GetTemplateSubitems(this.templateFolder, this.masterDb);
			foreach (Item item in templateSubitems)
			{
				ChecklistItem clItem = new ChecklistItem();
				clItem.ID = Control.GetUniqueID("I");
				clItem.Value = item.ID.ToString();
				clItem.Header = item.Name;
				TemplateList.Controls.Add(clItem);
			}

			TemplateList.CheckAll();
		}

		protected override void OnOK(object sender, EventArgs args)
		{
			foreach (ChecklistItem template in TemplateList.Items)
			{
				//Do nothing if the template has not been selected
				if (!template.Checked) continue;

				//Try and get the template
				TemplateItem templateItem = masterDb.GetItem(template.Value);
				if (templateItem == null) continue;

				//Using the settings item get the providers needed for creating both the namespaces for the custom items
				// as well as the file/folder strucutre.
				CustomItemSettings settings = new CustomItemSettings(HttpContext.Current);
				
				ICustomItemNamespaceProvider namespaceProvider =
					AssemblyUtil.GetNamespaceProvider(settings.NamespaceProvider);

				ICustomItemFolderPathProvider filePathProvider = AssemblyUtil.GetFilePathProvider(settings.FilepathProvider);

				//Get all the custom item information
				CustomItemInformation customItemInformation = new CustomItemInformation(templateItem, CustomItemNamespace.Value,
				                                                                        CustomItemFilePath.Value,
				                                                                        filePathProvider,
				                                                                        namespaceProvider);
				//Generate the class file(s)
				CodeGenerator codeGenerator =
					new CodeGenerator(customItemInformation,
					                  GenerateBaseFile.Checked, GenerateInstanceFile.Checked, GenerateInterfaceFile.Checked,
					                  GenerateStaticFile.Checked);

				codeGenerator.GenerateCode();
			}

			SheerResponse.Alert("Custom items sucessfully generated.", new string[0]);

			base.OnOK(sender, args);
		}
	}
}