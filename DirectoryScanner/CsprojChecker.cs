using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Exceptions;

namespace DirectoryScanner
{
	public class CsprojChecker : IFileChecker
	{
		private static readonly HashSet<string> _projects = new HashSet<string>();

		private static readonly IReadOnlyCollection<string> EmptyCollection = new String[0];

		private const StringComparison StrComp = StringComparison.InvariantCultureIgnoreCase;

		private static readonly object SyncRoot = new object();

		public IReadOnlyCollection<string> GetErrors(string file, HashSet<string> solutionFiles)
		{
			if (!file.EndsWith(".csproj"))
				return EmptyCollection;

			var result = new List<string>();

			lock (SyncRoot)
			{
				Project project;

				try
				{
					project = GetProject(file, solutionFiles);
				}
				catch (InvalidProjectFileException ex)
				{
					return new[] { ex.Message };
				}

				var references = project.GetItems("Reference");

				foreach (var reference in references)
				{
					string library = reference.EvaluatedInclude;
					var parts = library.Split(' ');

					bool isVersionSpecified = parts.Length > 1 && parts[0][parts[0].Length - 1] == ',' && parts[1].StartsWith("Version=", StrComp);

					if (isVersionSpecified)
						result.Add(library);

					var metadata = reference.Metadata;
					foreach (var metadataItem in metadata)
					{
						bool isSpecificVersionFalse = metadataItem.Name.Equals("SpecificVersion", StrComp) && metadataItem.EvaluatedValue.Equals("False", StrComp);
						if (isSpecificVersionFalse)
							result.Add("<SpecificVersion>False</SpecificVersion>");

						bool isHintPathLookingToBuildOutput = metadataItem.Name.Equals("HintPath", StrComp) && (
							metadataItem.EvaluatedValue.Contains(@"bin\Debug\") ||
							metadataItem.EvaluatedValue.Contains(@"bin\Release\"));

						if (isHintPathLookingToBuildOutput)
							result.Add(metadataItem.EvaluatedValue);

						//bool isHintPathLookingOutOfSolutionDirectory = metadataItem.Name.Equals("HintPath", strComp) &&
						//	Regex.IsMatch(metadataItem.EvaluatedValue, @"[^.]\\lib\\");

						//if (isHintPathLookingOutOfSolutionDirectory)
						//{
						//	AddCsprojError(path, metadataItem.EvaluatedValue);
						//	noError = false;
						//}
					}
				}

				return result;
			}
		}

		public bool FixErrors(string file, HashSet<string> solutionFiles)
		{
			if (!file.EndsWith(".csproj"))
				return false;

			bool modified = false;

			lock (SyncRoot)
				try
				{
					Project project = GetProject(file, solutionFiles);
					var references = project.GetItems("Reference");

					foreach (var reference in references)
					{
						string library = reference.EvaluatedInclude;
						var parts = library.Split(' ');

						bool isVersionSpecified = parts.Length > 1 && parts[0][parts[0].Length - 1] == ',' && parts[1].StartsWith("Version=", StrComp);

						if (isVersionSpecified)
						{
							var name = parts[0].TrimEnd(',');
							reference.UnevaluatedInclude = name;
							modified = true;
						}

						var toRemove = new List<ProjectMetadata>();
						var metadata = reference.DirectMetadata;
						foreach (var metadataItem in metadata)
						{
							bool isSpecificVersionFalse = metadataItem.Name.Equals("SpecificVersion", StrComp) && metadataItem.EvaluatedValue.Equals("False", StrComp);

							if (isSpecificVersionFalse)
								toRemove.Add(metadataItem);
						}

						foreach (var metadataItem in toRemove)
						{
							reference.RemoveMetadata(metadataItem.Name);
							modified = true;
						}
					}

					if (modified)
					{
						project.Save(file, Encoding.UTF8);
						return true;
					}
				}
				catch (InvalidProjectFileException)
				{
				}

			return false;
		}

		private Project GetProject(string path, HashSet<string> solutionFiles)
		{
			if (_projects.Contains(path))
			{
				_projects.Clear();
				// http://codeyourself.net/microsoft-build-evaluation-project-loading-oddities/
				ProjectCollection.GlobalProjectCollection.UnloadAllProjects();
			}

			string solutionDir = FindSolutionDir(path, solutionFiles);

			Project project;

			project = solutionDir != null 
				? new Project(path, new Dictionary<string, string> { { "SolutionDir", solutionDir } }, null) 
				: new Project(path);

			_projects.Add(path);

			return project;
		}

		private string FindSolutionDir(string path, HashSet<string> solutionFiles)
		{
			string currentDir = Path.GetDirectoryName(path);

			var solutionDirs = new HashSet<string>(solutionFiles.Select(Path.GetDirectoryName));

			while (currentDir != null)
			{
				if (solutionDirs.Contains(currentDir))
					return currentDir;

				currentDir = Path.GetDirectoryName(currentDir);
			}

			return null;
		}

		public string CheckerId { get { return "csproj references"; } }
	}
}