using System;
using System.Web;
using CustomItemGenerator.Settings;
using CustomItemGenerator.SitecoreApp;
using Sitecore.Web.UI.Sheer;
using Sitecore.Web.UI.HtmlControls;
using System.IO;
using CustomItemGenerator.Util;
using Sitecore.Data.Items;

namespace CustomItemGenerator.Pipelines {
    public class AutoUpdate : CustomItemCodeBeside {
        protected void OnItemSaved(object sender, EventArgs args) {

            CustomItemSettings settings = new CustomItemSettings(HttpContext.Current);

            // Only continue if we have valid args, auto update is set to true, and the file exists already
            if (args != null && settings.AutoUpdate) {

                // Get the template destination
                Item _temp = masterDb.GetItem(GetCurrentContentGuid());
                if (_temp != null) {
                    string file_path = FileUtil.GetClassFilePath(CodeUtil.GetClassNameForTemplate(_temp), settings.BaseFileOutputPath);

                    // Make sure the template already exists
                    if (File.Exists(file_path)) {

                        // Set these up since we don't have the editor open
                        CustomItemNamespace = new Edit();
                        CustomItemFilePath = new Edit();

                        // Call the current code beside
                        OnLoad(args);
                        OnOK(sender, args);
                    }
                }
            }

            return;
        }
    }
}
