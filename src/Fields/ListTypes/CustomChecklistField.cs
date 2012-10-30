using System.Collections.Generic;
using System.Linq;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace CustomItemGenerator.Fields.ListTypes
{
	public partial class CustomChecklistField : BaseCustomField<MultilistField>
	{

		public CustomChecklistField(Item item, MultilistField field)
			: base(item, field)
		{
		}

		public static implicit operator List<Item>(CustomChecklistField checklistField)
		{
			return checklistField.CheckedItems;
		}

		public List<Item> CheckedItems
		{
			get
			{
				if (field == null) return new List<Item>();
				if (item.Fields[field.InnerField.Name] == null) return new List<Item>();
				return ((MultilistField)item.Fields[field.InnerField.Name]).GetItems().ToList();
			}
		}
	}
}
