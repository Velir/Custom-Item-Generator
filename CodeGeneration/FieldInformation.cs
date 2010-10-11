using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CustomItemGenerator.Settings;
using CustomItemGenerator.Util;
using Sitecore.Data.Items;

namespace CustomItemGenerator.CodeGeneration
{
	public class FieldInformation
	{
		public string FieldName { get; private set; }
		public string FieldType { get; private set; }
		public string MethodName { get; private set; }

		public FieldInformation(string fieldName,string fieldType)
		{
			FieldName = fieldName;
			MethodName = fieldName.Replace(" ", string.Empty);
			FieldType = fieldType;
		}

		public FieldInformation(TemplateFieldItem fieldItem)
			: this(fieldItem.Name, TemplateUtil.GetFieldReturnType(fieldItem))
		{
		}
	}
}