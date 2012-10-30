using CustomItemGenerator.Util;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace CustomItemGenerator.Fields.SimpleTypes
{
	public partial class CustomImageField : BaseCustomField<ImageField>
	{
		public CustomImageField(Item item, ImageField field)
			: base(item, field)
		{
		}

		public static implicit operator MediaItem(CustomImageField imageField)
		{
			return ((imageField != null) ? imageField.MediaItem : null);
		}

		public MediaItem MediaItem
		{
			get
			{
				if (field == null) return null;
				if (item.Fields[field.InnerField.Name] == null) return null;
				return ((ImageField)item.Fields[field.InnerField.Name]).MediaItem;
			}
		}

		public string MediaUrl
		{
			get
			{
				return LinkUtil.GetMediaUrl(MediaItem);
			}
		}
	}
}
