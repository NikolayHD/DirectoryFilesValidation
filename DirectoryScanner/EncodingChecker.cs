using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Ude;

namespace DirectoryScanner
{
	public class EncodingChecker : IFileChecker
	{
		private static readonly object SyncRoot = new object();

		private static readonly IReadOnlyCollection<string> EmptyCollection = new String[0];

		private static readonly IReadOnlyCollection<string> IsAscii = new[] {"file is ascii encoded"};

		public IReadOnlyCollection<string> GetErrors(string file)
		{
			return isAscii(file) ? IsAscii : EmptyCollection;
		}

		public bool FixErrors(string file)
		{
			return convertToUtf8(file);
		}

		private bool isAscii(string fileName)
		{
			for (int i = 0; i < 10; i++)
			{
				try
				{
					lock (SyncRoot)
						using (FileStream fs = File.OpenRead(fileName))
						{
							var cdet = new CharsetDetector();
							cdet.Feed(fs);
							cdet.DataEnd();

							bool isAscii = EncodingChecker.isAscii(cdet);
							return isAscii;
						}
				}
				catch (IOException)
				{
					Thread.Sleep(1000);
				}
			}

			return false;
		}

		private static bool isAscii(CharsetDetector cdet)
		{
			return cdet.Confidence > 0.8 && cdet.Charset.StartsWith("windows-", StringComparison.InvariantCultureIgnoreCase);
		}

		private bool convertToUtf8(string fileName)
		{
			string charset;

			lock (SyncRoot)
				using (FileStream fs = File.OpenRead(fileName))
				{
					var cdet = new CharsetDetector();
					cdet.Feed(fs);
					cdet.DataEnd();

					charset = cdet.Charset;

					if (!isAscii(cdet))
						return false;
				}

			var srcEncoding = Encoding.GetEncoding(charset);

			var text = File.ReadAllText(fileName, srcEncoding);
			File.WriteAllText(fileName, text, Encoding.UTF8);
			return true;
		}

		public string CheckerId { get { return "ascii encoding"; } }
	}
}