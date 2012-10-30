using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CustomItemGenerator.Util;
using NVelocity;
using NVelocity.App;

namespace CustomItemGenerator.CodeGeneration
{
	public class CodeGenerator
	{
		public CustomItemInformation CustomItemInformation { get; private set; }

		//The user can choose which files to generate through the UI
		public bool GenerateBaseFile { get; private set; }
		public List<string> GeneratedFilePaths { get; private set; }
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
			GeneratedFilePaths = new List<string>();
		}

		public void GenerateCode()
		{
			VelocityEngine velocity = new VelocityEngine();

			TextReader reader = new StreamReader(NvelocityUtil.GetTemplateFolderPath() + "\\CustomItem.base.vm");
			string template = reader.ReadToEnd();

			//Setup the template with the needed code, and then do the merge	
			VelocityContext baseContext = new VelocityContext();

			baseContext.Put("Usings", CustomItemInformation.Usings);
			baseContext.Put("BaseTemplates", CustomItemInformation.BaseTemplates);
			baseContext.Put("CustomItemFields", CustomItemInformation.Fields);
			baseContext.Put("CustomItemInformation", CustomItemInformation);

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
					Velocity.Init();
					sw.Write(Sitecore.Text.NVelocity.VelocityHelper.Evaluate(baseContext, template, "base-custom-item"));
					GenerationMessage += filePath + " successfully written\n\n";
					GeneratedFilePaths.Add(filePath);
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

			GeneratedFilePaths.Add(filePath);
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
			TextReader reader = new StreamReader(NvelocityUtil.GetTemplateFolderPath() + "\\CustomItem.partial.vm");
			string template = reader.ReadToEnd();
			string folderPath = CustomItemInformation.FolderPathProvider.GetFolderPath(CustomItemInformation.Template,
			                                                                         CustomItemInformation.BaseFileRoot);

			VelocityContext partialContext = new VelocityContext();
			partialContext.Put("CustomItemInformation", CustomItemInformation);

			writer = new StringWriter();
			writer.Write(Sitecore.Text.NVelocity.VelocityHelper.Evaluate(partialContext, template, "base-custom-item"));

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

			string relativeNamespace = customItemInformation.FullNameSpace.Replace(customItemInformation.BaseNamespace, string.Empty);

			string[] dirNames = relativeNamespace.Split('.');
			string baseFileRoot = customItemInformation.BaseFileRoot;

			foreach (string dirName in dirNames)
			{
				if (!string.IsNullOrEmpty(dirName))
				{
					baseFileRoot = baseFileRoot + @"\" + dirName;
					if (!Directory.Exists(baseFileRoot))
					{
						Directory.CreateDirectory(baseFileRoot);
						if (!Directory.Exists(baseFileRoot))
						{
							throw new FileNotFoundException("Could not create directory: " + baseFileRoot);
						}
					}
				}
			}
		}

		public string DelimitedCreatedFilePathString
		{
			get
			{
				StringBuilder builder = new StringBuilder();
				string str = string.Empty;
				foreach (string str2 in GeneratedFilePaths)
				{
					builder.Append(str);
					builder.Append(str2);
					str = "|";
				}
				return builder.ToString();
			}
		}
	}
}