using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace CustomItemGenerator.Fields.SimpleTypes
{
	public partial class CustomCheckboxField : BaseCustomField<CheckboxField>
	{
		public CustomCheckboxField(Item item, CheckboxField field) : base(item, field)
		{
		}

		public bool Checked
		{
			get
			{
				if (field == null) return false;
				if (item.Fields[field.InnerField.Name] == null) return false;
				return ((CheckboxField)item.Fields[field.InnerField.Name]).Checked;
			}
		}
	}
}
