using System;
using System.Collections.Generic;
using DirectoryScanner;

namespace FixCsproj
{
	class Program
	{
		static int Main(string[] args)
		{
			var parsedArgs = GetParsedArgs(args);

			if (!parsedArgs.ContainsKey("-d"))
			{
				PrintUsage();
				return 0;
			}

			var directory = parsedArgs["-d"];

			var scanner = new Scanner(directory, "*.csproj", new CsprojChecker());
			
			if (parsedArgs.ContainsKey("-findOnly"))
			{
				scanner.Scan();
				return scanner.ErrorsFound ? 1 : 0;
			}

			Console.Out.WriteLine("fixing...");
			scanner.ScanAndFixErrrors();
			Console.WriteLine("done");

			return 0;
		}
		
		private static void PrintUsage()
		{
			var exeFile = "FixCsproj.exe";

			Console.Out.WriteLine(exeFile + " -d directory [-findOnly]");
			Console.Out.WriteLine("\t fix all errors in *.csproj files in directory recursively");
			Console.Out.WriteLine("\t -findOnly prevents fixing and modifies the return code to be 1 if errors were found");

			Console.Out.WriteLine(exeFile + " -h");
			Console.Out.WriteLine("\t this help");
		}

		private static Dictionary<string, string> GetParsedArgs(string[] args)
		{
			var result = new Dictionary<string, string>();

			for (int i = 0; i < args.Length; i++)
			{
				if (args[i].StartsWith("-"))
				{
					var key = args[i];

					var value = i + 1 < args.Length && !args[i + 1].StartsWith("-")
						? args[i + 1]
						: null;

					result.Add(key, value);
				}
			}
			
			return result;
		}
	}
}
