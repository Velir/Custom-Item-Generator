using Sitecore.Data.Items;
using CustomItemGenerator.Fields.SimpleTypes;

namespace CustomItemGenerator.CustomItems
{
	public partial class FieldMappingItem : CustomItem
	{
		public static readonly string TemplateId = "{B626796A-7483-4708-9127-66F6986D6AF3}";


		#region Boilerplate CustomItem Code

		public FieldMappingItem(Item innerItem)
			: base(innerItem)
		{

		}

		public static implicit operator FieldMappingItem(Item innerItem)
		{
			return innerItem != null ? new FieldMappingItem(innerItem) : null;
		}

		public static implicit operator Item(FieldMappingItem customItem)
		{
			return customItem != null ? customItem.InnerItem : null;
		}

		#endregion //Boilerplate CustomItem Code


		#region Field Instance Methods


		public CustomTextField FieldReturnType
		{
			get
			{
				return new CustomTextField(InnerItem, InnerItem.Fields["Field Return Type"]);
			}
		}


		#endregion //Field Instance Methods
	}
}