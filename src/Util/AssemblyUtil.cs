using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CustomItemGenerator.Interfaces;
using CustomItemGenerator.Providers;
using Sitecore.Diagnostics;
using Sitecore.Reflection;

namespace CustomItemGenerator.Util
{
	public class AssemblyUtil
	{

		public static ICustomItemFolderPathProvider GetFilePathProvider(string assemblyString)
		{
			TemplatePathFilePathProvider defaultProvider = new TemplatePathFilePathProvider();

			//Split the assembly string into it's parts, an example would look something like this
			// CustomItemGenerator.Providers.TemplatePathNameSpaceProvider, CustomItemGenerator
			string[] assemblyParts = assemblyString.Split(',');
			string classType = assemblyParts[0].Trim();
			string assemblyName = assemblyParts[1].Trim();

			//Try and retrieve the assembly
			Assembly assembly;
			try
			{
				assembly = ReflectionUtil.LoadAssembly(assemblyName);
			}
			catch (FileNotFoundException exception)
			{
				Log.Error(exception.Message, "ClassImplementsInterface");
				return defaultProvider;
			}
			if (assembly == null) return defaultProvider;

			//Try and retrieve the class type
			Type type = assembly.GetType(classType);
			if (type == null)
			{
				Log.Error("Type " + classType + " Not Found for " + assemblyString, "ClassImplementsInterface");
				return defaultProvider;
			}

			if (type.GetInterface(typeof(ICustomItemFolderPathProvider).FullName) == null) return defaultProvider;

			ICustomItemFolderPathProvider provider = (ICustomItemFolderPathProvider)Activator.CreateInstance(type);
			if (provider == null) return defaultProvider;

			return provider;
		}

		public static ICustomItemNamespaceProvider GetNamespaceProvider(string assemblyString)
		{
			TemplatePathNameSpaceProvider defaultProvider = new TemplatePathNameSpaceProvider();

			//Split the assembly string into it's parts, an example would look something like this
			// CustomItemGenerator.Providers.TemplatePathNameSpaceProvider, CustomItemGenerator
			string[] assemblyParts = assemblyString.Split(',');
			string classType = assemblyParts[0].Trim();
			string assemblyName = assemblyParts[1].Trim();

			//Try and retrieve the assembly
			Assembly assembly;
			try
			{
				assembly = ReflectionUtil.LoadAssembly(assemblyName);
			}
			catch (FileNotFoundException exception)
			{
				Log.Error(exception.Message, "ClassImplementsInterface");
				return defaultProvider;
			}
			if (assembly == null) return defaultProvider;

			//Try and retrieve the class type
			Type type = assembly.GetType(classType);
			if (type == null)
			{
				Log.Error("Type " + classType + " Not Found for " + assemblyString, "ClassImplementsInterface");
				return defaultProvider;
			}

			if (type.GetInterface(typeof(ICustomItemNamespaceProvider).FullName) == null) return defaultProvider;

			ICustomItemNamespaceProvider provider = (ICustomItemNamespaceProvider)Activator.CreateInstance(type);
			if (provider == null) return defaultProvider;

			return provider;
		}
	}
}
