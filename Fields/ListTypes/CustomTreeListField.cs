using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace CustomItemGenerator.Fields.ListTypes
{
	public partial class CustomTreeListField : CustomMultiListField
	{
		public CustomTreeListField(Item item, MultilistField field) : base(item, field)
		{
	
		}
	}
}
