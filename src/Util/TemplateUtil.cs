using System.Collections.Generic;
using System.Linq;
using System.Web;
using CustomItemGenerator.CodeGeneration;
using CustomItemGenerator.Settings;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace CustomItemGenerator.Util
{
	public class TemplateUtil
	{
		/// <summary>
		/// Using the sitecore settings, return the proper return type for the passed in
		/// field.
		/// </summary>
		/// <param name="field">The field.</param>
		/// <returns></returns>
		public static string GetFieldReturnType(TemplateFieldItem field)
		{
			string fieldType = field.Type.ToLower();

			CustomItemSettings settings = new CustomItemSettings(HttpContext.Current);

			foreach (FieldMapping fieldMapping in settings.FieldMappings)
			{
				if (fieldMapping.SitecoreFieldType.ToLower() != fieldType) continue;
				return fieldMapping.FieldReturnType;
			}

			return string.Empty;
		}

		/// <summary>
		/// Gets all user fields, these are the fields that do not start with __.
		/// </summary>
		/// <param name="itemTemplate">The item template.</param>
		/// <returns></returns>
		private static List<TemplateFieldItem> GetAllUserFields(TemplateItem itemTemplate)
		{
			//Loop through all of the fields, if the field starts with __ it is assumed that
			//the field is a system field and willl not be returned from this method.
			return itemTemplate.OwnFields.Where(templateFieldItem => !templateFieldItem.Name.StartsWith("__")).ToList();
		}

		/// <summary>
		/// Returns a list of objects that describe all of the non system fields in a template.
		/// </summary>
		/// <param name="itemTemplate">The item template.</param>
		/// <returns></returns>
		public static List<FieldInformation> GetTemplateFieldInformation(TemplateItem itemTemplate)
		{
			List<TemplateFieldItem> templateFields = GetAllUserFields(itemTemplate);
			return templateFields.Select(templateField => new FieldInformation(templateField)).ToList();
		}

		/// <summary>
		/// Gets the subitems below a root item taht are Sitecore templates 
		/// </summary>
		/// <param name="rootItem">The root item.</param>
		/// <param name="database">The database.</param>
		/// <returns></returns>
		public static List<Item> GetTemplateSubitems(Item rootItem, Database database)
		{
			TemplateItem sitecoreTemplateItem = database.GetItem("{AB86861A-6030-46C5-B394-E8F99E8B87DB}");

			return (from Item child in rootItem.Axes.GetDescendants()
							where child.Template.ID == sitecoreTemplateItem.ID
							select child).OrderBy(i => i.Name).ToList();
		}

		/// <summary>
		/// Returns the relative path to a template.  The assumption being made as far as 
		/// the template folder structure is that the user templates will be contained in
		/// a top level folder and will not be directly under the /sitecore/templates folder.
		/// For example with the path /sitecore/templates/My Templates/Template Folder 2/Template 2 , the relative path
		/// would be TemplateFolder2 since the My Templates folder is considered the template
		/// root for our used templates.
		/// </summary>
		/// <param name="template">The template.</param>
		/// <returns></returns>
		public static string GetRelativeTemplatePath(TemplateItem template)
		{
			//Build the relative path starting using the path of the folder that this template is in, then
			// strip out the base part of the folder path to get the relative path
			string relativePath = template.InnerItem.Paths.ParentPath;
			relativePath = relativePath.Replace("/sitecore/templates/", string.Empty);

			//If there are no slashes left in the path it means that the template is at 
			//our user template root, thus there is no path.
			int index = relativePath.IndexOf("/");
			if (index < 0) return string.Empty;

			//Otherwise return the path minus the first slash
			return relativePath.Substring(index + 1);
		}
	}
}