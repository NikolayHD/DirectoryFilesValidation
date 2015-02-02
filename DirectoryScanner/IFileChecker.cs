using System.Collections.Generic;

namespace DirectoryScanner
{
	public interface IFileChecker
	{
		IReadOnlyCollection<string> GetErrors(string file);

		bool FixErrors(string file);

		string CheckerId { get; }
	}
}