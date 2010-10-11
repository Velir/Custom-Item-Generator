using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace CustomItemGenerator.Fields.ListTypes
{
	public partial class CustomDroplistField : BaseCustomField<GroupedDroplistField>
	{
		public CustomDroplistField(Item item, GroupedDroplistField field)
			: base(item, field)
		{
		}


	}
}
