using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CustomItemGenerator.Settings
{
	public class FieldMapping
	{
		private string _sitecoreFieldType;
		private string _fieldReturnType;

		public string SitecoreFieldType
		{
			get { return _sitecoreFieldType; }
		}

		public string FieldReturnType
		{
			get { return _fieldReturnType; }
		}

		public FieldMapping(string sitecoreFieldType, string fieldReturnType)
		{
			_sitecoreFieldType = sitecoreFieldType;
			_fieldReturnType = fieldReturnType;
		}

		public FieldMapping(XmlNode mappingNode)
		{
			_sitecoreFieldType = mappingNode.Attributes["fieldType"].Value;
			_fieldReturnType = mappingNode.InnerText;
		}
	}
}
