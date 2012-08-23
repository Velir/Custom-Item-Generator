using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace CustomItemGenerator.Fields.SimpleTypes
{
	public partial class CustomTextField : BaseCustomField<TextField>
	{
		public CustomTextField(Item item, TextField field)
			: base(item, field)
		{
		}

		public static implicit operator string(CustomTextField textField)
		{
			return ((textField != null) ? textField.Text : null);
		}

		public string Text
		{
			get { return Rendered; }
		}
	}
}
