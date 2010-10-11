using System;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace CustomItemGenerator.Fields.SimpleTypes
{
	public partial class CustomDateField : BaseCustomField<DateField>
	{
		public CustomDateField(Item item, DateField field)
			: base(item, field)
		{
		}

		public DateTime DateTime
		{
			get
			{
				if (field == null) return DateTime.MinValue;
				if (item.Fields[field.InnerField.Name] == null) return DateTime.MinValue;
				return ((DateField)item.Fields[field.InnerField.Name]).DateTime;
			}
		}
	}
}
