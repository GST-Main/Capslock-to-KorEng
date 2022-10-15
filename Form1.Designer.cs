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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.contextMenuStrip_Tray = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripMenuItem_RunPause = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem_Quit = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuStrip_Tray.SuspendLayout();
			this.SuspendLayout();
			// 
			// notifyIcon
			// 
			this.notifyIcon.ContextMenuStrip = this.contextMenuStrip_Tray;
			this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
			this.notifyIcon.Text = "CapsLock To KorEng";
			this.notifyIcon.Visible = true;
			// 
			// contextMenuStrip_Tray
			// 
			this.contextMenuStrip_Tray.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_RunPause,
            this.toolStripMenuItem_Quit});
			this.contextMenuStrip_Tray.Name = "contextMenuStrip1";
			this.contextMenuStrip_Tray.Size = new System.Drawing.Size(106, 48);
			// 
			// toolStripMenuItem_RunPause
			// 
			this.toolStripMenuItem_RunPause.Name = "toolStripMenuItem_RunPause";
			this.toolStripMenuItem_RunPause.Size = new System.Drawing.Size(105, 22);
			this.toolStripMenuItem_RunPause.Text = "Pause";
			this.toolStripMenuItem_RunPause.Click += new System.EventHandler(this.toolStripMenuItem_RunPause_Click);
			// 
			// toolStripMenuItem_Quit
			// 
			this.toolStripMenuItem_Quit.Name = "toolStripMenuItem_Quit";
			this.toolStripMenuItem_Quit.Size = new System.Drawing.Size(105, 22);
			this.toolStripMenuItem_Quit.Text = "Quit";
			this.toolStripMenuItem_Quit.Click += new System.EventHandler(this.toolStripMenuItem_Quit_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(244, 141);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Form1";
			this.Text = "Capslock To KorEng";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Shown += new System.EventHandler(this.Form1_Shown);
			this.contextMenuStrip_Tray.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private NotifyIcon notifyIcon;
		private ContextMenuStrip contextMenuStrip_Tray;
		private ToolStripMenuItem toolStripMenuItem_RunPause;
		private ToolStripMenuItem toolStripMenuItem_Quit;
	}
}