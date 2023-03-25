namespace Capslock_to_KorEng
{
	partial class Form1
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			notifyIcon = new NotifyIcon(components);
			contextMenuStrip_Tray = new ContextMenuStrip(components);
			toolStripMenuItem_RunPause = new ToolStripMenuItem();
			toolStripMenuItem_Quit = new ToolStripMenuItem();
			toolStripMenuItem_LaunchOnStartup = new ToolStripMenuItem();
			contextMenuStrip_Tray.SuspendLayout();
			SuspendLayout();
			// 
			// notifyIcon
			// 
			notifyIcon.ContextMenuStrip = contextMenuStrip_Tray;
			notifyIcon.Icon = (Icon)resources.GetObject("notifyIcon.Icon");
			notifyIcon.Text = "CapsLock To KorEng";
			notifyIcon.Visible = true;
			// 
			// contextMenuStrip_Tray
			// 
			contextMenuStrip_Tray.Items.AddRange(new ToolStripItem[] { toolStripMenuItem_LaunchOnStartup, toolStripMenuItem_RunPause, toolStripMenuItem_Quit });
			contextMenuStrip_Tray.Name = "contextMenuStrip1";
			contextMenuStrip_Tray.Size = new Size(181, 92);
			// 
			// toolStripMenuItem_RunPause
			// 
			toolStripMenuItem_RunPause.Name = "toolStripMenuItem_RunPause";
			toolStripMenuItem_RunPause.Size = new Size(180, 22);
			toolStripMenuItem_RunPause.Text = "Pause";
			toolStripMenuItem_RunPause.Click += toolStripMenuItem_RunPause_Click;
			// 
			// toolStripMenuItem_Quit
			// 
			toolStripMenuItem_Quit.Name = "toolStripMenuItem_Quit";
			toolStripMenuItem_Quit.Size = new Size(180, 22);
			toolStripMenuItem_Quit.Text = "Quit";
			toolStripMenuItem_Quit.Click += toolStripMenuItem_Quit_Click;
			// 
			// toolStripMenuItem_LaunchOnStartup
			// 
			toolStripMenuItem_LaunchOnStartup.Name = "toolStripMenuItem_LaunchOnStartup";
			toolStripMenuItem_LaunchOnStartup.Size = new Size(180, 22);
			toolStripMenuItem_LaunchOnStartup.Text = "Launch on Startup";
			toolStripMenuItem_LaunchOnStartup.Click += toolStripMenuItem_LaunchOnStartup_Click;
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(244, 141);
			FormBorderStyle = FormBorderStyle.FixedSingle;
			Icon = (Icon)resources.GetObject("$this.Icon");
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "Form1";
			Text = "Capslock To KorEng";
			FormClosing += Form1_FormClosing;
			Load += Form1_Load;
			Shown += Form1_Shown;
			contextMenuStrip_Tray.ResumeLayout(false);
			ResumeLayout(false);
		}

		#endregion

		private NotifyIcon notifyIcon;
		private ContextMenuStrip contextMenuStrip_Tray;
		private ToolStripMenuItem toolStripMenuItem_RunPause;
		private ToolStripMenuItem toolStripMenuItem_Quit;
		private ToolStripMenuItem toolStripMenuItem_LaunchOnStartup;
	}
}