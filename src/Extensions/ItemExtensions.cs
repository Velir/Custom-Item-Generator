using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Data.Templates;

namespace CustomItemGenerator.Extensions
{
    public static class ItemExtensions
    {
        public static bool IsDerivedFrom(this Item item, string templateId)
        {
            if (item == null)
                return false;

            if (string.IsNullOrEmpty(templateId))
            {
                return false;
            }

            Item templateItem = item.Database.Items[new ID(templateId)];
            if (templateItem == null)
            {
                return false;
            }

            Template template = TemplateManager.GetTemplate(item);
            if (template == null)
            {
                return false;
            }

            if (template.ID == templateItem.ID)
            {
                return true;
            }

            return template.DescendsFrom(templateItem.ID);
        }
    }
}
