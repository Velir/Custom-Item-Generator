using CustomItemGenerator.Util;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace CustomItemGenerator.Fields.SimpleTypes
{
	public partial class CustomFileField : BaseCustomField<FileField>
	{
		public CustomFileField(Item item, FileField field)
			: base(item, field)
		{
		}

		public static implicit operator MediaItem(CustomFileField fileField)
		{
			return ((fileField != null) ? fileField.MediaItem : null);
		}

		public MediaItem MediaItem
		{
			get
			{
				if (field == null) return null;
				if (item.Fields[field.InnerField.Name] == null) return null;
				return ((FileField)item.Fields[field.InnerField.Name]).MediaItem;
			}
		}

		public string MediaUrl
		{
			get { return LinkUtil.GetMediaUrl(MediaItem); }
		}
	}
}
