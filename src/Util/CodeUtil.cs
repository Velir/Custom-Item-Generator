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

			//If the template name ends with the word "item" do not append item
			//onto the class name, this avoids situations such as templateItemItem
			if (!template.Name.ToLower().EndsWith("item"))
			{
				className = template.Name + "Item";
			}
			else
			{
				className = template.Name;
			}
			
			//Strip out spaces and dashes from the class name
			className = CleanStringOfIllegalCharacters(className);

			return className;
		}
		
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

		public static string CleanStringOfIllegalCharacters(string inputString)
		{
			inputString = inputString.Replace(" ", string.Empty);
			inputString = inputString.Replace("-", string.Empty);
			return inputString;
		}

		
	}
}
