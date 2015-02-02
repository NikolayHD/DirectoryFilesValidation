using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Windows.Forms;
using DirectoryFilesValidationGui;
using DirectoryScanner;

namespace AsciiFileDetector
{
	public partial class FormApp : Form
	{
		private Scanner _scanner;
		private FormWindowState _lastState;
		private readonly Timer _timer;
		
		public FormApp()
		{
			InitializeComponent();
			this.notifyIcon1.Icon = this.Icon = Resource.IconAnsii;
			this.WindowState = FormWindowState.Minimized;
			this.ShowInTaskbar = false;
			_lastState = this.WindowState;

			_timer = new Timer
			{
				Interval = 5000,
			};

			_timer.Tick += (x, y) => TimerTick();

			_timer.Start();
		}

		private void TimerTick()
		{
			RefreshTitle();
			RefreshList();
		}

		private void RefreshTitle()
		{
			string text = string.Empty;

			if (_scanner.Scanning)
				text = "[scanning...] ";

			bool errorsFound = false;

			foreach (var checkerId in _scanner.CheckerIdList)
			{
				var badFiles = _scanner.GetFilesWithErrors(checkerId);
				if (badFiles.Count > 0)
				{
					errorsFound = true;
					text += string.Format(" {0}: {1} file(s)", checkerId, badFiles.Count);
				}
			}

			if (!errorsFound)
				text += " clean";

			this.notifyIcon1.Text = this.Text = text;

			this.notifyIcon1.Icon = this.Icon =
				errorsFound
					? Resource.ProgressWarn
					: Resource.IconAnsii;
		}

		private void RefreshList()
		{
			var result = new StringBuilder();

			foreach (var checkerId in _scanner.CheckerIdList)
			{
				foreach (var file in _scanner.GetFilesWithErrors(checkerId))
				{
					var errors = _scanner.GetErrors(checkerId, file);

					if (errors.Count > 0)
					{
						result.AppendLine(file);

						foreach (var error in errors)
						{
							result.Append("\t");
							result.AppendLine(error);
						}
					}
				}
			}

			var text = result.ToString();

			var selectionStart = this.richTextBox1.SelectionStart;
			var selectionLength = this.richTextBox1.SelectionLength;
			
			this.richTextBox1.Text = text;

			if (selectionStart <= text.Length && selectionStart + selectionLength <= text.Length)
			{
				this.richTextBox1.SelectionStart = selectionStart;
				this.richTextBox1.SelectionLength = selectionLength;
			}
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			RestartMonitor();
		}

		private void RestartMonitor()
		{
			CloseMonitor();

			Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

			string directory = config.AppSettings.Settings["Directory"].Value;
			string pattern = config.AppSettings.Settings["Pattern"].Value;

			this.textBoxDir.Text = directory;
			this.textBoxPattern.Text = pattern;

			_scanner = new Scanner(directory, pattern, new CsprojChecker(), new EncodingChecker());
			_scanner.Start();
		}

		private void CloseMonitor()
		{
			if (_scanner != null)
			{
				_scanner.Stop();
			}
		}
		
		private void buttonApplyConfig_Click(object sender, EventArgs e)
		{
			SaveConfig();
			RestartMonitor();
		}

		private void SaveConfig()
		{
			Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

			config.AppSettings.Settings.Remove("Directory");
			config.AppSettings.Settings.Add("Directory", textBoxDir.Text);

			config.AppSettings.Settings.Remove("Pattern");
			config.AppSettings.Settings.Add("Pattern", textBoxPattern.Text);

			config.Save(ConfigurationSaveMode.Full);
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing)
			{
				e.Cancel = true;
				this.WindowState = FormWindowState.Minimized;
				return;
			}

			_timer.Stop();
			CloseMonitor();
		}

		private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
				return;

			if (this.WindowState == FormWindowState.Minimized)
			{
				this.ShowInTaskbar = true;
				this.WindowState = FormWindowState.Normal;
				this.Activate();
			}
			else
			{
				this.WindowState = FormWindowState.Minimized;
				this.ShowInTaskbar = false;
			}
		}

		protected override void OnClientSizeChanged(EventArgs e)
		{
			if (this.WindowState != _lastState)
			{
				_lastState = this.WindowState;
				OnWindowStateChanged(e);
			}
			base.OnClientSizeChanged(e);
		}
		protected void OnWindowStateChanged(EventArgs e)
		{
			if (WindowState == FormWindowState.Minimized)
				this.ShowInTaskbar = false;
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void buttonOpenFiles_Click(object sender, EventArgs e)
		{
			var files = this.richTextBox1.SelectedText.Split('\n');

			foreach (var file in files)
			{
				if (file.StartsWith("\t"))
					continue;
				
				var path = file.Trim();
				if (File.Exists(path))
					System.Diagnostics.Process.Start(path);
			}
		}

		private void buttonFixErrors_Click(object sender, EventArgs e)
		{
			_scanner.FixFoundErrors();
		}
	}
}
