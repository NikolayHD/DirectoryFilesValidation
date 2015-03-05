using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DirectoryScanner
{
	public class Scanner
	{
		const string SolutionFileExtension = ".sln";
		private static readonly IReadOnlyCollection<string> EmptyCollection = new ReadOnlyCollection<string>(new List<string>(0));
		private static readonly object SyncRoot = new object();

		private readonly HashSet<string> _filesQueue = new HashSet<string>();

		private readonly HashSet<string> _solutionFiles = new HashSet<string>();

		private readonly Dictionary<string, IFileChecker> _checkersById = new Dictionary<string, IFileChecker>();
		private readonly Dictionary<string, Dictionary<string, IReadOnlyCollection<string>>> _errorsByCheckerAndFile = new Dictionary<string, Dictionary<string, IReadOnlyCollection<string>>>();

		private readonly string _directory;
		private readonly string _pattern;

		private bool _scanning;
		private bool _processing;

		private bool _stopProcessing;


		private void AddErrorList(string checkerId, string file, IReadOnlyCollection<string> errorList)
		{
			lock (SyncRoot)
			{
				Dictionary<string, IReadOnlyCollection<string>> errorsByFile;

				if (!_errorsByCheckerAndFile.TryGetValue(checkerId, out errorsByFile))
				{
					errorsByFile = new Dictionary<string, IReadOnlyCollection<string>>();
					_errorsByCheckerAndFile.Add(checkerId, errorsByFile);
				}

				errorsByFile.Add(file, errorList);
			}
		}

		private void RemoveErrorList(string checkerId, string file)
		{
			lock (SyncRoot)
			{
				Dictionary<string, IReadOnlyCollection<string>> errorsByFile;

				if (!_errorsByCheckerAndFile.TryGetValue(checkerId, out errorsByFile))
					return;

				errorsByFile.Remove(file);
			}
		}

		public IReadOnlyCollection<string> CheckerIdList
		{
			get { return _checkersById.Keys.ToList(); }
		}

		public IReadOnlyCollection<string> GetFilesWithErrors(string checkerId)
		{
			lock (SyncRoot)
			{
				Dictionary<string, IReadOnlyCollection<string>> errorsByFile;
				if (_errorsByCheckerAndFile.TryGetValue(checkerId, out errorsByFile))
					return errorsByFile.Keys.ToList();

				return EmptyCollection;
			}
		}

		public IReadOnlyCollection<string> GetErrors(string checkerId, string file)
		{
			lock (SyncRoot)
			{
				Dictionary<string, IReadOnlyCollection<string>> errorsByFile;

				if (!_errorsByCheckerAndFile.TryGetValue(checkerId, out errorsByFile))
					return EmptyCollection;

				IReadOnlyCollection<string> errors;

				if (!errorsByFile.TryGetValue(file, out errors))
					return EmptyCollection;

				return errors;
			}
		}

		public bool ErrorsFound
		{
			get
			{
				lock (SyncRoot)
					return _errorsByCheckerAndFile
						.Select(pair => pair.Value.Values)
						.Any(errorLists => errorLists.Any(l => l.Count > 0));
			}
		}

		public bool Scanning
		{
			get { return _scanning; }
		}

		public bool Processing
		{
			get { return _processing; }
		}

		public Scanner(string directory, string pattern, IEnumerable<IFileChecker> fileCheckers)
		{
			_directory = directory;
			_pattern = pattern;
			_checkersById = fileCheckers.ToDictionary(c => c.CheckerId);
		}

		private void AddFileToQueue(string path)
		{
			lock (SyncRoot)
			{
				_filesQueue.Add(path);
			}
		}

		private string TakeFileFromQueue()
		{
			lock (SyncRoot)
			{
				if (_filesQueue.Count == 0)
					return null;

				var file = _filesQueue.First();
				_filesQueue.Remove(file);
				return file;
			}
		}

		public void Start(int scanInterval)
		{
			Task.Run(async () =>
			{
				while (!_stopProcessing)
				{
					ResetFoundFilesAndErrors();

					_scanning = true;
					findMatchingFiles();
					_scanning = false;

					if (_stopProcessing)
						break;

					_processing = true;
					processFilesQueue();
					_processing = false;

					if (_stopProcessing)
						break;

					await Task.Delay(scanInterval);
				}
			});
		}

		private void ResetFoundFilesAndErrors()
		{
			lock (SyncRoot)
			{
				foreach (var checkerId in _checkersById.Keys)
				{
					Dictionary<string, IReadOnlyCollection<string>> errorsByFile;
					if (_errorsByCheckerAndFile.TryGetValue(checkerId, out errorsByFile))
						errorsByFile.Clear();
				}

				_filesQueue.Clear();

				_solutionFiles.Clear();
			}
		}

		private void findMatchingFiles()
		{
			IEnumerable<string> patternParts = _pattern.Split('|');
			
			if (!patternParts.Contains("*" + SolutionFileExtension))
				patternParts = patternParts.Concat(new[] {"*" + SolutionFileExtension});

			foreach (var patternPart in patternParts)
			{
				var files = Directory.GetFiles(_directory, patternPart, SearchOption.AllDirectories);

				foreach (var file in files)
				{
					AddFileToQueue(file);
					if (file.EndsWith(SolutionFileExtension))
						_solutionFiles.Add(file);
				}
			}
		}

		public void FindErrors()
		{
			findMatchingFiles();
			processFilesQueue();
		}

		private void processFilesQueue()
		{
			string nextFile;

			do
			{
				nextFile = TakeFileFromQueue();

				if (nextFile != null)
					foreach (var pair in _checkersById)
					{
						var errors = pair.Value.GetErrors(nextFile, _solutionFiles);

						if (errors.Count > 0)
							AddErrorList(pair.Key, nextFile, errors);
					}
			} while (nextFile != null && !_stopProcessing);
		}

		public void Stop()
		{
			_stopProcessing = true;
		}

		public void FixFoundErrors(HashSet<string> ignoredFiles = null)
		{
			ignoredFiles = ignoredFiles ?? new HashSet<string>();

			Task.Run(() =>
			{
				foreach (var checkerId in CheckerIdList)
				{
					foreach (var file in this.GetFilesWithErrors(checkerId))
					{
						if (ignoredFiles.Contains(file))
							continue;

						var modified = _checkersById[checkerId].FixErrors(file, _solutionFiles);

						RemoveErrorList(checkerId, file);

						if (modified)
							AddFileToQueue(file);
					}
				}
			});
		}

		public void ScanAndFixErrrors()
		{
			ResetFoundFilesAndErrors();

			foreach (var file in Directory.GetFiles(_directory, "*." + SolutionFileExtension, SearchOption.AllDirectories))
				_solutionFiles.Add(file);

			foreach (var patternPart in _pattern.Split('|'))
				foreach (var file in Directory.GetFiles(_directory, patternPart, SearchOption.AllDirectories))
					foreach (var pair in _checkersById)
						pair.Value.FixErrors(file, _solutionFiles);
		}
	}
}