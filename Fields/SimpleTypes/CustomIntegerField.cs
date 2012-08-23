using System;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace CustomItemGenerator.Fields.SimpleTypes
{
	public partial class CustomIntegerField : BaseCustomField<TextField>
	{
		public CustomIntegerField(Item item, TextField field)
			: base(item, field)
		{
		}

		public static implicit operator int(CustomIntegerField intField)
		{
			return intField.Integer;
		}

		public int Integer
		{
			get
			{
				if (field == null) return int.MinValue;

				if (item.Fields[field.InnerField.Name] == null) return int.MinValue;

				int intValue;
				if (Int32.TryParse(item[field.InnerField.Name], out intValue))
				{
					return intValue;
				}

				return int.MinValue;
			}
		}
	}
}
