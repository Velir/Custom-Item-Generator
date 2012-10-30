using Sitecore;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;

namespace CustomItemGenerator.SitecoreApp
{
	public class CustomItemGeneratorCommand : Command
	{
		public override void Execute(CommandContext context)
		{
			if (context.Items.Length != 1) return;
			
			//TODO put the GUIDS used here somewhere central
			//If this is a template, launch the single template generator
			if (context.Items[0].TemplateID.ToString() == "{AB86861A-6030-46C5-B394-E8F99E8B87DB}")
			{
				//Build the url for the control
				string controlUrl = UIUtil.GetUri("control:xmlGenerateCustomItem");
				string id = context.Items[0].ID.ToString();
				string url = string.Format("{0}&id={1}", controlUrl, id);

				//Open the dialog
				Context.ClientPage.ClientResponse.Broadcast(Context.ClientPage.ClientResponse.ShowModalDialog(url), "Shell");
			}
				
				//If this is a template folder, launch the folder template generateor
			else if (context.Items[0].TemplateID.ToString() == "{0437FEE2-44C9-46A6-ABE9-28858D9FEE8C}")
			{
				//Build the url for the control
				string controlUrl = UIUtil.GetUri("control:xmlGenerateCustomItemByFolder");
				string id = context.Items[0].ID.ToString();
				string url = string.Format("{0}&id={1}", controlUrl, id);

				//Open the dialog
				Context.ClientPage.ClientResponse.Broadcast(Context.ClientPage.ClientResponse.ShowModalDialog(url), "Shell");
			}
			else
			{
				SheerResponse.Alert("You can only run the Custom Item Generator on a Template or Template Folder.", new string[0]);
			}
			
		}
	}
}