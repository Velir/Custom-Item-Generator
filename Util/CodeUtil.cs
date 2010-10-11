using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Commons.Collections;
using CustomItemGenerator.CodeGeneration;
using CustomItemGenerator.Fields;
using CustomItemGenerator.Fields.LinkTypes;
using CustomItemGenerator.Fields.ListTypes;
using CustomItemGenerator.Fields.SimpleTypes;
using NVelocity;
using NVelocity.App;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Template=Sitecore.Data.Templates.Template;

namespace CustomItemGenerator.Util
{
	public class CodeUtil
	{
		public static string GetClassNameForTemplate(TemplateItem template)
		{
			string className = string.Empty;

			//If the template name ends with the word item do not append item
			//onto the class name, this avoids situations such as templateItemItem
			if (template.Name.ToLower().EndsWith("item"))
			{
				className = template.Name.Replace(" ", string.Empty);
			}
			else
			{
				className = template.Name.Replace(" ", string.Empty) + "Item";
			}

			//If the class starts with a _, then strip it off, this is used for base templates
			//and is not what we want in code
			/*if (className.StartsWith("_"))
			{
				className = className.Substring(1);
			}
			*/
			return className;
		}
		/*
		public static string GetCustomFieldTypeName(TemplateFieldItem field)
		{
			string fieldType = field.Type.ToLower();

			//TODO should password fields be different
			//Text Fields
			if (
				fieldType == "rich text" ||
				fieldType == "single-line text" ||
				fieldType == "multi-line text" ||
				fieldType == "password")
			{
				return typeof(CustomTextField).Name;
			}

			//File Field
			if (
				fieldType == "file")
			{
				return typeof(CustomFileField).Name;
			}
			
			//Image Field
			if (fieldType == "image")
			{
				return typeof(CustomImageField).Name;
			}

			//Date Fields
			if (
				fieldType == "date" ||
				fieldType == "datetime")
			{
				return typeof(CustomDateField).Name;
			}

			//Checkbox
			if (fieldType == "checkbox")
			{
				return typeof(CustomCheckboxField).Name;
			}

			//Checklist
			if (fieldType == "checklist")
			{
				return typeof(CustomChecklistField).Name;
			}

			//Drop Tree
			if (fieldType == "droptree" ||
				fieldType == "droplink")
			{
				return typeof (CustomLookupField).Name;
			}

			//General Link
			if (fieldType == "general link")
			{
				return typeof(CustomGeneralLinkField).Name;
			}

			//Multilist
			if (fieldType == "multilist" ||
				fieldType =="sortingmultilist" ||
				fieldType == "droplist")
			{
				return typeof(CustomMultiListField).Name;
			}

			//Treelist
			if (fieldType == "treelist" ||
				fieldType == "treelistex")
			{
				return typeof(CustomTreeListField).Name;
			}

			//TODO Figure out what to do with the number field, a double?
			//Integer
			if (
				fieldType == "integer" ||
				fieldType == "number")
			{
				return typeof(CustomIntegerField).Name;
			}


			return string.Empty;
		}
		*/
		public static string GetCodeFileString(HttpServerUtility server, CustomItemInformation info)
		{
			VelocityEngine velocity = new VelocityEngine();

			ExtendedProperties props = new ExtendedProperties();
			props.SetProperty("file.resource.loader.path", server.MapPath(".")); // The base path for Templates 

			velocity.Init(props);

			//Template template = velocity.GetTemplate("template.tmp");
			NVelocity.Template template = velocity.GetTemplate("CustomItem.vm");

			VelocityContext context = new VelocityContext();

			context.Put("BaseTemplates", info.BaseTemplates);
			context.Put("CustomItemFields", info.Fields);
			context.Put("CustomItemInformation", info);

			StringWriter writer = new StringWriter();
			template.Merge(context, writer);
			return writer.GetStringBuilder().ToString();
		}

		public static void WriteCodeFile(string fileNameAndPath, string fileContents)
		{
			
		}

		
	}
}
