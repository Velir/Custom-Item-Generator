using System;
using Sitecore.Data.Items;
using System.Collections.Generic;
using Sitecore.Data.Fields;
using Sitecore.Web.UI.WebControls;
using CustomItemGenerator.Fields.LinkTypes;
using CustomItemGenerator.Fields.ListTypes;
using CustomItemGenerator.Fields.SimpleTypes;

namespace CustomItemGenerator.CustomItems
{
public partial class CustomItemSettingsItem : CustomItem
{

public static readonly string TemplateId = "{1F646612-47F9-48D2-8BB7-B96E0C279305}";


#region Boilerplate CustomItem Code

public CustomItemSettingsItem(Item innerItem) : base(innerItem)
{

}

public static implicit operator CustomItemSettingsItem(Item innerItem)
{
	return innerItem != null ? new CustomItemSettingsItem(innerItem) : null;
}

public static implicit operator Item(CustomItemSettingsItem customItem)
{
	return customItem != null ? customItem.InnerItem : null;
}

#endregion //Boilerplate CustomItem Code


#region Field Instance Methods


public CustomTextField BaseNamespace
{
	get
	{
		return new CustomTextField(InnerItem, InnerItem.Fields["Base Namespace"]);
	}
}


public CustomTextField NamespaceProvider
{
	get
	{
		return new CustomTextField(InnerItem, InnerItem.Fields["Namespace Provider"]);
	}
}


public CustomTextField BaseFileOutputPath
{
	get
	{
		return new CustomTextField(InnerItem, InnerItem.Fields["Base File Output Path"]);
	}
}


public CustomTextField FilepathProvider
{
	get
	{
		return new CustomTextField(InnerItem, InnerItem.Fields["Filepath Provider"]);
	}
}


public CustomTextField NvelocityTemplatePath
{
	get
	{
		return new CustomTextField(InnerItem, InnerItem.Fields["Nvelocity Template Path"]);
	}
}


#endregion //Field Instance Methods
}
}