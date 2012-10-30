using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace CustomItemGenerator.Fields.LinkTypes
{
	public partial class CustomLookupField : BaseCustomField<LookupField>
	{
		public CustomLookupField(Item item, LookupField field) : base(item, field)
		{
		}

		public static implicit operator Item(CustomLookupField lookupField)
		{
			return lookupField.Item;
		}

		public Item Item
		{
			get
			{
				if(field == null) return null;
				return field.TargetItem;
			}
		}
	}
}
