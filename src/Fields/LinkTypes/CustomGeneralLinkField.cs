using CustomItemGenerator.Util;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace CustomItemGenerator.Fields.LinkTypes
{
	public partial class CustomGeneralLinkField : BaseCustomField<LinkField>
	{
		public CustomGeneralLinkField(Item item, LinkField field)
			: base(item, field)
		{
		}

		public static implicit operator string(CustomGeneralLinkField generalLinkField)
		{
			return generalLinkField.Url;
		}

		public string Url
		{
			get
			{
				if (field == null) return null;
				if (item.Fields[field.InnerField.Name] == null) return null;
				return (LinkUtil.GetLinkFieldUrl(item.Fields[field.InnerField.Name]));
			}
		}
	}
}
