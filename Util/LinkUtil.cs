using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;

namespace CustomItemGenerator.Util
{
	public class LinkUtil 
	{
		public static string GetMediaUrl(MediaItem mediaItem)
		{
			if (mediaItem == null) return string.Empty;
			return Sitecore.StringUtil.EnsurePrefix('/', Sitecore.Resources.Media.MediaManager.GetMediaUrl(mediaItem));
		}

		public static string GetLinkFieldUrl(LinkField field)
		{
			if (field == null) return string.Empty;

			//If it is an internal link return the URL to the item
			if (field.IsInternal)
			{
				Item targetItem = Sitecore.Context.Database.GetItem(field.TargetID);
				if(targetItem == null) return string.Empty;

				if (targetItem.Paths.IsMediaItem)
                		{
                    			return Sitecore.StringUtil.EnsurePrefix('/', Sitecore.Resources.Media.MediaManager.GetMediaUrl(targetItem));
                		}

				return LinkManager.GetItemUrl(targetItem);
			}

			//If it is a media link, return the media path
			if (field.IsMediaLink)
			{
				if (field.TargetItem == null) return string.Empty;
				return Sitecore.StringUtil.EnsurePrefix('/', Sitecore.Resources.Media.MediaManager.GetMediaUrl(field.TargetItem));
			}

			//Return the url if it is not a 
			if (field.Url == null) return string.Empty;
			return field.Url;
		}
	}
}
