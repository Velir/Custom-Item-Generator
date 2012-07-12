using System.Collections.Generic;
using System.Linq;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace CustomItemGenerator.Fields.ListTypes
{
	public partial class CustomMultiListField : BaseCustomField<MultilistField>
	{
		public CustomMultiListField(Item item, MultilistField field)
			: base(item, field)
		{
		}

		public List<Item> ListItems
		{
			get
			{
				if (field == null) return new List<Item>();
				if (item.Fields[field.InnerField.Name] == null) return new List<Item>();
				return ((MultilistField)item.Fields[field.InnerField.Name]).GetItems().ToList();
			}
		}

		/// <summary>
		/// Returns the ID values of the field as a list of strings
		/// </summary>
		public List<string> Ids
		{
			get
			{
				if (field == null)
				{
					return new List<string>();
				}

				if (item.Fields[field.InnerField.Name] == null)
				{
					return new List<string>();
				}

				if (string.IsNullOrEmpty(Raw))
				{
					return new List<string>();
				}

				List<string> itemIds = new List<string>();
				foreach(string id in Raw.Split('|'))
				{
					if(string.IsNullOrEmpty(id))
					{
						continue;
					}

					itemIds.Add(id);
				}

				return itemIds;
			}
		}
	}
}
