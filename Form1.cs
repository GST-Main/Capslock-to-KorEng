using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;

namespace Capslock_to_KorEng
{
	public partial class Form1 : Form
	{
		private readonly uint KEYDOWN = 0x1;
		private readonly uint KEYUP = 0x2;

		// Hotkeys
		[DllImport("user32.dll")]
		public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
		[DllImport("user32.dll")]
		public static extern bool UngegisterHotKey(IntPtr hWnd, int id);

		// Send Key
		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("user32.dll")]
		static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

		const int myHotKeyID = 666;

		private BackgroundWorker worker;

		public Form1()
		{
			InitializeComponent();
			RegisterHotKey(this.Handle, myHotKeyID, 0, (int)Keys.CapsLock);
			worker = new BackgroundWorker();
		}

		private void worker_DoWork(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker bw = sender as BackgroundWorker;

			int arg = (int)e.Argument;
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 0x0312 && m.WParam.ToInt32() == myHotKeyID)
			{
				Thread.Sleep(50);

				keybd_event(byte.Parse(((int)Keys.KanaMode).ToString()), 0x45, KEYDOWN, UIntPtr.Zero);

				Thread.Sleep(50);

				keybd_event(byte.Parse(((int)Keys.KanaMode).ToString()), 0x45, KEYUP, UIntPtr.Zero);
			}
			base.WndProc(ref m);
		}
	}
}