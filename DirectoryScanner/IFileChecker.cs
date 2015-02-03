using System.Collections.Generic;

namespace DirectoryScanner
{
	public interface IFileChecker
	{
		IReadOnlyCollection<string> GetErrors(string file, HashSet<string> solutionFiles);

		bool FixErrors(string file, HashSet<string> solutionFiles);

		string CheckerId { get; }
	}
}