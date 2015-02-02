namespace AsciiFileDetector
{
	partial class FormApp
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.textBoxDir = new System.Windows.Forms.TextBox();
			this.textBoxPattern = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.buttonApplyConfig = new System.Windows.Forms.Button();
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.scanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.buttonOpenCsproj = new System.Windows.Forms.Button();
			this.buttonFixCsproj = new System.Windows.Forms.Button();
			this.buttonIgnore = new System.Windows.Forms.Button();
			this.buttonResetIgnore = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// richTextBox1
			// 
			this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.richTextBox1.Location = new System.Drawing.Point(12, 125);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.ReadOnly = true;
			this.richTextBox1.Size = new System.Drawing.Size(579, 342);
			this.richTextBox1.TabIndex = 3;
			this.richTextBox1.Text = "";
			// 
			// textBoxDir
			// 
			this.textBoxDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxDir.Location = new System.Drawing.Point(70, 13);
			this.textBoxDir.Name = "textBoxDir";
			this.textBoxDir.Size = new System.Drawing.Size(521, 20);
			this.textBoxDir.TabIndex = 4;
			// 
			// textBoxPattern
			// 
			this.textBoxPattern.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxPattern.Location = new System.Drawing.Point(70, 39);
			this.textBoxPattern.Name = "textBoxPattern";
			this.textBoxPattern.Size = new System.Drawing.Size(520, 20);
			this.textBoxPattern.TabIndex = 5;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(52, 13);
			this.label1.TabIndex = 6;
			this.label1.Text = "Directory:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(32, 42);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(32, 13);
			this.label2.TabIndex = 7;
			this.label2.Text = "Filter:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(9, 109);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(83, 13);
			this.label3.TabIndex = 8;
			this.label3.Text = "Detected errors:";
			// 
			// buttonApplyConfig
			// 
			this.buttonApplyConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonApplyConfig.Location = new System.Drawing.Point(512, 65);
			this.buttonApplyConfig.Name = "buttonApplyConfig";
			this.buttonApplyConfig.Size = new System.Drawing.Size(79, 23);
			this.buttonApplyConfig.TabIndex = 9;
			this.buttonApplyConfig.Text = "Scan";
			this.buttonApplyConfig.UseVisualStyleBackColor = true;
			this.buttonApplyConfig.Click += new System.EventHandler(this.buttonApplyConfig_Click);
			// 
			// notifyIcon1
			// 
			this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
			this.notifyIcon1.Text = "Ansii files detector";
			this.notifyIcon1.Visible = true;
			this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem,
            this.scanToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(100, 48);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// scanToolStripMenuItem
			// 
			this.scanToolStripMenuItem.Name = "scanToolStripMenuItem";
			this.scanToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
			this.scanToolStripMenuItem.Text = "Scan";
			this.scanToolStripMenuItem.Click += new System.EventHandler(this.buttonApplyConfig_Click);
			// 
			// buttonOpenCsproj
			// 
			this.buttonOpenCsproj.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOpenCsproj.Location = new System.Drawing.Point(466, 473);
			this.buttonOpenCsproj.Name = "buttonOpenCsproj";
			this.buttonOpenCsproj.Size = new System.Drawing.Size(124, 23);
			this.buttonOpenCsproj.TabIndex = 10;
			this.buttonOpenCsproj.Text = "Open selected files";
			this.buttonOpenCsproj.UseVisualStyleBackColor = true;
			this.buttonOpenCsproj.Click += new System.EventHandler(this.buttonOpenFiles_Click);
			// 
			// buttonFixCsproj
			// 
			this.buttonFixCsproj.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonFixCsproj.Location = new System.Drawing.Point(336, 473);
			this.buttonFixCsproj.Name = "buttonFixCsproj";
			this.buttonFixCsproj.Size = new System.Drawing.Size(124, 23);
			this.buttonFixCsproj.TabIndex = 11;
			this.buttonFixCsproj.Text = "Fix all";
			this.buttonFixCsproj.UseVisualStyleBackColor = true;
			this.buttonFixCsproj.Click += new System.EventHandler(this.buttonFixErrors_Click);
			// 
			// buttonIgnore
			// 
			this.buttonIgnore.Location = new System.Drawing.Point(12, 473);
			this.buttonIgnore.Name = "buttonIgnore";
			this.buttonIgnore.Size = new System.Drawing.Size(126, 23);
			this.buttonIgnore.TabIndex = 12;
			this.buttonIgnore.Text = "Ignore selected files";
			this.buttonIgnore.UseVisualStyleBackColor = true;
			this.buttonIgnore.Click += new System.EventHandler(this.buttonIgnore_Click);
			// 
			// buttonResetIgnore
			// 
			this.buttonResetIgnore.Location = new System.Drawing.Point(144, 473);
			this.buttonResetIgnore.Name = "buttonResetIgnore";
			this.buttonResetIgnore.Size = new System.Drawing.Size(75, 23);
			this.buttonResetIgnore.TabIndex = 13;
			this.buttonResetIgnore.Text = "Reset ignore";
			this.buttonResetIgnore.UseVisualStyleBackColor = true;
			this.buttonResetIgnore.Click += new System.EventHandler(this.buttonResetIgnore_Click);
			// 
			// FormApp
			// 
			this.AcceptButton = this.buttonApplyConfig;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(603, 508);
			this.Controls.Add(this.buttonResetIgnore);
			this.Controls.Add(this.buttonIgnore);
			this.Controls.Add(this.buttonFixCsproj);
			this.Controls.Add(this.buttonOpenCsproj);
			this.Controls.Add(this.buttonApplyConfig);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBoxPattern);
			this.Controls.Add(this.textBoxDir);
			this.Controls.Add(this.richTextBox1);
			this.Name = "FormApp";
			this.ShowInTaskbar = false;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.RichTextBox richTextBox1;
		private System.Windows.Forms.TextBox textBoxDir;
		private System.Windows.Forms.TextBox textBoxPattern;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button buttonApplyConfig;
		private System.Windows.Forms.NotifyIcon notifyIcon1;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.Button buttonOpenCsproj;
		private System.Windows.Forms.Button buttonFixCsproj;
		private System.Windows.Forms.ToolStripMenuItem scanToolStripMenuItem;
		private System.Windows.Forms.Button buttonIgnore;
		private System.Windows.Forms.Button buttonResetIgnore;
		private System.Windows.Forms.ToolTip toolTip1;
	}
}

