using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;

using System;
using System.Diagnostics;

namespace Capslock_to_KorEng
{
	public partial class Form1 : Form
	{
		private GlobalKeyboardHook _globalKeyboardHook;

		private const int VKLShift = 0xA0;
		private const int VKCapslock = 0x14;

		private bool isLShiftPressed = false;

		// Hotkeys
		[DllImport("user32.dll")]
		public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
		[DllImport("user32.dll")]
		public static extern bool UngegisterHotKey(IntPtr hWnd, int id);

		// Send Key
		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("user32.dll")]
		static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

		//private BackgroundWorker worker;

		public Form1()
		{
			InitializeComponent();
			//RegisterHotKey(this.Handle, myHotKeyID, 0, (int)Keys.CapsLock);
			//worker = new BackgroundWorker();
			SetupKeyboardHooks();
		}

		//private void worker_DoWork(object sender, DoWorkEventArgs e)
		//{
		//	BackgroundWorker bw = sender as BackgroundWorker;

		//	int arg = (int)e.Argument;
		//}

		public void SetupKeyboardHooks()
		{
			_globalKeyboardHook = new GlobalKeyboardHook(new Keys[] { Keys.CapsLock, Keys.LShiftKey });
			_globalKeyboardHook.KeyboardPressed += OnKeyPressed;
			_globalKeyboardHook.KeyboardPressed += SetLShiftStatus;
		}

		private void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
		{
			Debug.WriteLine(e.KeyboardData.VirtualCode);
			Debug.WriteLine(e.KeyboardData.Flags);
			Debug.WriteLine(e.KeyboardState);

			if (e.KeyboardData.VirtualCode != VKCapslock)
				return;
			else if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyDown && isLShiftPressed)
			{
				return;
			}
			else if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyDown)
			{
				e.Handled = true;
			}
		}

		private void SetLShiftStatus(object sender, GlobalKeyboardHookEventArgs e)
		{
			if (e.KeyboardData.VirtualCode != VKLShift)
				return;
			else if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyDown)
			{
				isLShiftPressed = true;
			}
			else if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyUp)
			{
				isLShiftPressed = false;
			}
		}
	}
}