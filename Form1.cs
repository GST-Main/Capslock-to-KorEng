using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Capslock_to_KorEng
{
	public partial class Form1 : Form
	{
		private bool isPaused = false;

		private GlobalKeyboardHook _globalKeyboardHook;

		private const byte VKLShift = 0xA0;
		private const byte VKCapslock = 0x14;
		private const byte VKKanaMode = 0x15;

		private const uint WM_KEYDOWN = 0x1;
		private const uint WM_KEYUP = 0x2;

		private bool isLShiftPressed = false;
		private bool launchOnStartup;

		// Send Key
		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("user32.dll")]
		static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

		public Form1()
		{
			InitializeComponent();
			SetupKeyboardHooks();
			launchOnStartup = GetStartupLaunch();
			toolStripMenuItem_LaunchOnStartup.Checked = launchOnStartup;
			if (launchOnStartup)
				UpdateStartupLaunchPath();
		}

		public void SetupKeyboardHooks()
		{
			_globalKeyboardHook = new GlobalKeyboardHook(new Keys[] { Keys.CapsLock, Keys.LShiftKey });
			_globalKeyboardHook.KeyboardPressed += OnKeyPressed;
			_globalKeyboardHook.KeyboardPressed += SetLShiftStatus;
		}

		private void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
		{
			if (!isPaused)
			{
				//Debug.WriteLine(e.KeyboardData.VirtualCode);
				//Debug.WriteLine(e.KeyboardData.Flags);
				//Debug.WriteLine(e.KeyboardState);

				if (e.KeyboardData.VirtualCode != VKCapslock)
					return;
				else if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyDown && isLShiftPressed)
				{
					return;
				}
				else if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyDown)
				{
					e.Handled = true;

					keybd_event(VKKanaMode, 0x45, WM_KEYDOWN, UIntPtr.Zero);
					Thread.Sleep(50);
					keybd_event(VKKanaMode, 0x45, WM_KEYUP, UIntPtr.Zero);
				}
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

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = true;
			Hide();
			notifyIcon.Visible = true;
		}

		private void toolStripMenuItem_Quit_Click(object sender, EventArgs e)
		{
			Environment.Exit(0);
		}

		private void toolStripMenuItem_RunPause_Click(object sender, EventArgs e)
		{
			isPaused = !isPaused;
			if (isPaused)
				toolStripMenuItem_RunPause.Text = "Run";
			else
				toolStripMenuItem_RunPause.Text = "Pause";
		}

		private void Form1_Shown(object sender, EventArgs e)
		{
			Hide();
		}

		private void SetStartupLaunch(bool launchOnStartup)
		{
			RegistryKey? rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

			if (launchOnStartup)
				rk?.SetValue(Application.ProductName, AppDomain.CurrentDomain.BaseDirectory);
			else
				rk?.DeleteValue(Application.ProductName, throwOnMissingValue: false);
		}

		private bool GetStartupLaunch()
		{
			RegistryKey? rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
			if (rk != null)
			{
				string? value = (string?)rk.GetValue(Application.ProductName);
				if (value != null)
					return true;
			}

			return false;
		}

		private void UpdateStartupLaunchPath()
		{
			RegistryKey? rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
			if (rk != null)
			{
				string? currentPath = (string?)rk.GetValue(Application.ProductName);
				if (currentPath != null && currentPath != AppDomain.CurrentDomain.BaseDirectory)
					rk?.SetValue(Application.ProductName, AppDomain.CurrentDomain.BaseDirectory);
			}
		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}

		private void toolStripMenuItem_LaunchOnStartup_Click(object sender, EventArgs e)
		{
			SetStartupLaunch(!launchOnStartup);
			launchOnStartup = GetStartupLaunch();
			toolStripMenuItem_LaunchOnStartup.Checked = launchOnStartup;
		}
	}
}