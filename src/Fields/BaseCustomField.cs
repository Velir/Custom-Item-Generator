using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Web.UI.WebControls;

namespace CustomItemGenerator.Fields
{
	public abstract class BaseCustomField<T> where T : CustomField
	{
		protected T field;
		protected Item item;

		protected BaseCustomField(Item item, T field)
		{
			this.field = field;
			this.item = item;
		}

		public string Raw
		{
			get
			{
				if (field == null) return string.Empty;
				return field.Value;
			}
		}

		public string Rendered
		{
			get
			{
				if (field == null) return string.Empty;
				return FieldRenderer.Render(item, field.InnerField.Name);
			}
		}

		public string RenderFormatted(string format)
        	{
            		var output = this.Rendered;
            		return !string.IsNullOrWhiteSpace(output) ? string.Format(format, output) : string.Empty;
        	}

		public T Field
		{
			get
			{
				return field;
			}
		}
	}
}
