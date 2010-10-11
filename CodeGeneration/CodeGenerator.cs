using System;
using System.IO;
using Commons.Collections;
using CustomItemGenerator.CodeGeneration;
using CustomItemGenerator.Util;
using NVelocity;
using NVelocity.App;

namespace CustomItemGenerator
{
	public class CodeGenerator
	{
		public CustomItemInformation CustomItemInformation { get; private set; }

		//The user can choose which files to generate through the UI
		public bool GenerateBaseFile { get; private set; }
		public bool GenerateInstanceFile { get; private set; }
		public bool GenerateInterfaceFile { get; private set; }
		public bool GenerateStaticFile { get; private set; }
		public string GenerationMessage { get; private set; }

		public CodeGenerator(CustomItemInformation customItemInformation, bool generateBaseFile,
		                     bool generateInstanceFile, bool generateInterfaceFile, bool generateStaticFile)
		{
			CustomItemInformation = customItemInformation;
			GenerateBaseFile = generateBaseFile;
			GenerateInstanceFile = generateInstanceFile;
			GenerateInterfaceFile = generateInterfaceFile;
			GenerateStaticFile = generateStaticFile;
			GenerationMessage = string.Empty;
		}

		public void GenerateCode()
		{
			VelocityEngine velocity = new VelocityEngine();


			//Set the file.resource.loader.path property so that nvelocity can find the 
			//custom item template files
			ExtendedProperties props = new ExtendedProperties();
			props.SetProperty("file.resource.loader.path", NvelocityUtil.GetTemplateFolderPath());
			velocity.Init(props);

			//TODO extra checking to make sure we get back a template
			//Attempt to load the NVelocity template file
			Template baseTemplate = velocity.GetTemplate("CustomItem.base.vm");

			//Setup the template with the needed code, and then do the merge
			VelocityContext baseContext = new VelocityContext();

			baseContext.Put("Usings", CustomItemInformation.Usings);
			baseContext.Put("BaseTemplates", CustomItemInformation.BaseTemplates);
			baseContext.Put("CustomItemFields", CustomItemInformation.Fields);
			baseContext.Put("CustomItemInformation", CustomItemInformation);

			StringWriter writer = new StringWriter();
			baseTemplate.Merge(baseContext, writer);

			//Get the full file path to the .base.cs file
			string filePath = FileUtil.GetClassFilePath(CustomItemInformation.ClassName,
			                                            CustomItemInformation.FolderPathProvider.GetFolderPath(
			                                            	CustomItemInformation.Template, CustomItemInformation.BaseFileRoot));
			


			//Build the folder strucutre so that we have a place to put the .base.cs file
			BuildFolderStructure(CustomItemInformation);

			//Write the .base.cs file
			if (GenerateBaseFile)
			{
				using (StreamWriter sw = new StreamWriter(filePath))
				{
					//TODO add error checking
					sw.Write(writer.GetStringBuilder().ToString());
					GenerationMessage += filePath + " successfully written\n\n";
				}
			}

			//Write out the other partial files
			OuputPartialFiles(velocity);
		}

		/// <summary>
		/// Outputs a file if does not already exist.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		/// <param name="fileContents">The file contents.</param>
		private bool OutputFileIfDoesNotExist(string filePath, StringWriter fileContent)
		{
			//Only create the file if it does not already exist
			if (File.Exists(filePath)) 
			{
				GenerationMessage += filePath + " already exists\n\n";
				return false;
			}

			using (StreamWriter sw = new StreamWriter(filePath))
			{
				try
				{
					sw.Write(fileContent.GetStringBuilder().ToString());
				}
				catch (Exception e)
				{
					GenerationMessage += filePath + " writing failed : " + e.Message+ "\n\n";
					return false;
				}
				
			}

			GenerationMessage += filePath + " successfully written\n\n";
			return true;
		}


		/// <summary>
		/// Ouputs the partial class files for a custom item.
		/// </summary>
		/// <param name="velocity">The velocity.</param>
		private void OuputPartialFiles(VelocityEngine velocity)
		{
			StringWriter writer;
			Template partialTemplate = velocity.GetTemplate("CustomItem.partial.vm");
			string folderPath = CustomItemInformation.FolderPathProvider.GetFolderPath(CustomItemInformation.Template,
			                                                                         CustomItemInformation.BaseFileRoot);

			VelocityContext partialContext = new VelocityContext();
			partialContext.Put("CustomItemInformation", CustomItemInformation);

			writer = new StringWriter();
			partialTemplate.Merge(partialContext, writer);

			//.instance.cs
			if (GenerateInstanceFile)
			{
				//Only create the file if it does not already exist
				string instanceFilePath = folderPath + "\\" + CustomItemInformation.ClassName + ".instance.cs";
				OutputFileIfDoesNotExist(instanceFilePath, writer);
			}

			//.interface.cs
			if (GenerateInterfaceFile)
			{
				//Only create the file if it does not already exist
				string interfaceFilePath = folderPath + "\\" + CustomItemInformation.ClassName + ".interface.cs";
				OutputFileIfDoesNotExist(interfaceFilePath, writer);
			}

			//.static.cs
			if (GenerateStaticFile)
			{
				//Only create the file if it does not already exist
				string staticFilePath = folderPath + "\\" + CustomItemInformation.ClassName + ".static.cs";
				OutputFileIfDoesNotExist(staticFilePath, writer);
			}
		}

		private void BuildFolderStructure(CustomItemInformation customItemInformation)
		{
			bool baseDirectoryExists = Directory.Exists(customItemInformation.BaseFileRoot);
			if (!baseDirectoryExists)
			{
				throw new DirectoryNotFoundException("Base Directory Not Found: " + customItemInformation.BaseFileRoot);
			}

			string relativeNamespace = customItemInformation.FullNameSpace.Replace(customItemInformation.BaseNamespace,
			                                                                       string.Empty);

			string[] dirNames = relativeNamespace.Split('.');
			string relativeDirectory = customItemInformation.BaseFileRoot;

			foreach (string dirName in dirNames)
			{
				if (string.IsNullOrEmpty(dirName)) continue;

				relativeDirectory = relativeDirectory + "\\" + dirName;
				if (!Directory.Exists(relativeDirectory))
				{
					Directory.CreateDirectory(relativeDirectory);
					if (!Directory.Exists(relativeDirectory))
					{
						throw new FileNotFoundException("Could not create directory: " + relativeDirectory);
					}
				}
			}
		}
	}
}