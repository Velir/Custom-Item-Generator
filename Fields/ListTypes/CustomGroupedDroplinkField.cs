using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace CustomItemGenerator.Fields.ListTypes
{
	public partial class CustomGroupedDroplinkField: BaseCustomField<GroupedDroplinkField>
	{
		public CustomGroupedDroplinkField(Item item, GroupedDroplinkField field)
			: base(item, field)
		{
		}

		public Item Item
		{
			get
			{
				if (field == null) return null;
				if (item.Fields[field.InnerField.Name] == null) return null;
				return ((GroupedDroplinkField)item.Fields[field.InnerField.Name]).TargetItem;
			}
		}
	}
}
