namespace CustomItemGenerator.Attributes
{
	using System;

	[AttributeUsage(AttributeTargets.All)]
	public class SitecoreTemplateIdAttribute : Attribute
	{
		public readonly string TemplateId;

		public SitecoreTemplateIdAttribute(string templateId)
		{
			TemplateId = templateId;
		}
	}
}

