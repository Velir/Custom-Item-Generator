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

		public string Text
		{
			get { return Rendered; }
		}
	}
}
