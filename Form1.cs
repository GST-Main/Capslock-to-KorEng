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
			_globalKeyboardHook = new GlobalKeyboardHook();
			_globalKeyboardHook.KeyboardPressed += OnKeyPressed;
		}

		private void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
		{
			Debug.WriteLine(e.KeyboardData.VirtualCode);
			Debug.WriteLine(e.KeyboardData.Flags);
			Debug.WriteLine(e.KeyboardState);

			if (e.KeyboardData.VirtualCode != GlobalKeyboardHook.VKCapslock)
				return;
			if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyDown && e.KeyboardData.VirtualCode == GlobalKeyboardHook.LShift)
			{
				return;
			}
			if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyDown)
			{
				e.Handled = true;
			}
		}
	}

	// ********************************************************
	class GlobalKeyboardHookEventArgs : HandledEventArgs
	{
		public GlobalKeyboardHook.KeyboardState KeyboardState { get; private set; }
		public GlobalKeyboardHook.LowLevelKeyboardInputEvent KeyboardData { get; private set; }

		public GlobalKeyboardHookEventArgs(
			GlobalKeyboardHook.LowLevelKeyboardInputEvent keyboardData,
			GlobalKeyboardHook.KeyboardState keyboardState)
		{
			KeyboardData = keyboardData;
			KeyboardState = keyboardState;
		}
	}

	//Based on https://gist.github.com/Stasonix
	class GlobalKeyboardHook : IDisposable
	{
		public event EventHandler<GlobalKeyboardHookEventArgs> KeyboardPressed;

		public GlobalKeyboardHook()
		{
			_windowsHookHandle = IntPtr.Zero;
			_user32LibraryHandle = IntPtr.Zero;
			_hookProc = LowLevelKeyboardProc; // we must keep alive _hookProc, because GC is not aware about SetWindowsHookEx behaviour.

			_user32LibraryHandle = LoadLibrary("User32");
			if (_user32LibraryHandle == IntPtr.Zero)
			{
				int errorCode = Marshal.GetLastWin32Error();
				throw new Win32Exception(errorCode, $"Failed to load library 'User32.dll'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
			}



			_windowsHookHandle = SetWindowsHookEx(WH_KEYBOARD_LL, _hookProc, _user32LibraryHandle, 0);
			if (_windowsHookHandle == IntPtr.Zero)
			{
				int errorCode = Marshal.GetLastWin32Error();
				throw new Win32Exception(errorCode, $"Failed to adjust keyboard hooks for '{Process.GetCurrentProcess().ProcessName}'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
			}
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				// because we can unhook only in the same thread, not in garbage collector thread
				if (_windowsHookHandle != IntPtr.Zero)
				{
					if (!UnhookWindowsHookEx(_windowsHookHandle))
					{
						int errorCode = Marshal.GetLastWin32Error();
						throw new Win32Exception(errorCode, $"Failed to remove keyboard hooks for '{Process.GetCurrentProcess().ProcessName}'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
					}
					_windowsHookHandle = IntPtr.Zero;

					// ReSharper disable once DelegateSubtraction
					_hookProc -= LowLevelKeyboardProc;
				}
			}

			if (_user32LibraryHandle != IntPtr.Zero)
			{
				if (!FreeLibrary(_user32LibraryHandle)) // reduces reference to library by 1.
				{
					int errorCode = Marshal.GetLastWin32Error();
					throw new Win32Exception(errorCode, $"Failed to unload library 'User32.dll'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
				}
				_user32LibraryHandle = IntPtr.Zero;
			}
		}

		~GlobalKeyboardHook()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private IntPtr _windowsHookHandle;
		private IntPtr _user32LibraryHandle;
		private HookProc _hookProc;

		delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

		[DllImport("kernel32.dll")]
		private static extern IntPtr LoadLibrary(string lpFileName);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		private static extern bool FreeLibrary(IntPtr hModule);

		/// <summary>
		/// The SetWindowsHookEx function installs an application-defined hook procedure into a hook chain.
		/// You would install a hook procedure to monitor the system for certain types of events. These events are
		/// associated either with a specific thread or with all threads in the same desktop as the calling thread.
		/// </summary>
		/// <param name="idHook">hook type</param>
		/// <param name="lpfn">hook procedure</param>
		/// <param name="hMod">handle to application instance</param>
		/// <param name="dwThreadId">thread identifier</param>
		/// <returns>If the function succeeds, the return value is the handle to the hook procedure.</returns>
		[DllImport("USER32", SetLastError = true)]
		static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, int dwThreadId);

		/// <summary>
		/// The UnhookWindowsHookEx function removes a hook procedure installed in a hook chain by the SetWindowsHookEx function.
		/// </summary>
		/// <param name="hhk">handle to hook procedure</param>
		/// <returns>If the function succeeds, the return value is true.</returns>
		[DllImport("USER32", SetLastError = true)]
		public static extern bool UnhookWindowsHookEx(IntPtr hHook);

		/// <summary>
		/// The CallNextHookEx function passes the hook information to the next hook procedure in the current hook chain.
		/// A hook procedure can call this function either before or after processing the hook information.
		/// </summary>
		/// <param name="hHook">handle to current hook</param>
		/// <param name="code">hook code passed to hook procedure</param>
		/// <param name="wParam">value passed to hook procedure</param>
		/// <param name="lParam">value passed to hook procedure</param>
		/// <returns>If the function succeeds, the return value is true.</returns>
		[DllImport("USER32", SetLastError = true)]
		static extern IntPtr CallNextHookEx(IntPtr hHook, int code, IntPtr wParam, IntPtr lParam);

		[StructLayout(LayoutKind.Sequential)]
		public struct LowLevelKeyboardInputEvent
		{
			/// <summary>
			/// A virtual-key code. The code must be a value in the range 1 to 254.
			/// </summary>
			public int VirtualCode;

			/// <summary>
			/// A hardware scan code for the key. 
			/// </summary>
			public int HardwareScanCode;

			/// <summary>
			/// The extended-key flag, event-injected Flags, context code, and transition-state flag. This member is specified as follows. An application can use the following values to test the keystroke Flags. Testing LLKHF_INJECTED (bit 4) will tell you whether the event was injected. If it was, then testing LLKHF_LOWER_IL_INJECTED (bit 1) will tell you whether or not the event was injected from a process running at lower integrity level.
			/// </summary>
			public int Flags;

			/// <summary>
			/// The time stamp stamp for this message, equivalent to what GetMessageTime would return for this message.
			/// </summary>
			public int TimeStamp;

			/// <summary>
			/// Additional information associated with the message. 
			/// </summary>
			public IntPtr AdditionalInformation;
		}

		public const int WH_KEYBOARD_LL = 13;
		//const int HC_ACTION = 0;

		public enum KeyboardState
		{
			KeyDown = 0x0100,
			KeyUp = 0x0101,
			SysKeyDown = 0x0104,
			SysKeyUp = 0x0105
		}

		public const int VkSnapshot = 0x2c;
		//const int VkLwin = 0x5b;
		//const int VkRwin = 0x5c;
		//const int VkTab = 0x09;
		//const int VkEscape = 0x18;
		//const int VkControl = 0x11;
		const int KfAltdown = 0x2000;
		public const int LlkhfAltdown = (KfAltdown >> 8);

		public const int LShift = 0xA0;
		public const int VKCapslock = 0x14;
		public const int fCapslock = (VKCapslock >> 8);

		public IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam)
		{
			bool fEatKeyStroke = false;

			var wparamTyped = wParam.ToInt32();
			if (Enum.IsDefined(typeof(KeyboardState), wparamTyped))
			{
				object o = Marshal.PtrToStructure(lParam, typeof(LowLevelKeyboardInputEvent));
				LowLevelKeyboardInputEvent p = (LowLevelKeyboardInputEvent)o;

				var eventArguments = new GlobalKeyboardHookEventArgs(p, (KeyboardState)wparamTyped);

				EventHandler<GlobalKeyboardHookEventArgs> handler = KeyboardPressed;
				handler?.Invoke(this, eventArguments);

				fEatKeyStroke = eventArguments.Handled;
			}

			return fEatKeyStroke ? (IntPtr)1 : CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
		}
	}
}