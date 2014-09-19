using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Microsoft.BariVsPackage.BariExtension
{
    internal class KeyboardHook : IDisposable
    {
        private Action<Keys> handleKeyPressed;
        private HookProc keyboardHookProcedure;
        private readonly IntPtr hHook;

        public KeyboardHook(Action<Keys> handleKeyPressed)
        {
            this.handleKeyPressed = handleKeyPressed;

            keyboardHookProcedure = KeyboardHookProc;

#pragma warning disable 612,618
            hHook = SetWindowsHookEx(WH_KEYBOARD, keyboardHookProcedure, IntPtr.Zero,
                                     (uint) AppDomain.GetCurrentThreadId());
#pragma warning restore 612,618
            if (hHook == IntPtr.Zero)
            {
                var error = Marshal.GetLastWin32Error();
                throw new Win32Exception(error);
            }
        }

        private delegate int HookProc(int code, IntPtr wParam, IntPtr lParam);
        
        [DllImport("user32.dll", EntryPoint = "SetWindowsHookEx", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", EntryPoint = "UnhookWindowsHookEx", SetLastError = true)]
        private static extern IntPtr UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        private static extern int CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        public int KeyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            var keyCode = wParam.ToInt32();
            if (typeof (Keys).IsEnumDefined(keyCode))
            {
                handleKeyPressed((Keys) keyCode);
            }
            return CallNextHookEx(hHook, nCode, wParam, lParam);
        }

        // ReSharper disable InconsistentNaming
        private const int WH_KEYBOARD = 2;
        // ReSharper restore InconsistentNaming

        public void Dispose()
        {
            UnhookWindowsHookEx(hHook);
            keyboardHookProcedure = null;
            handleKeyPressed = null;
        }
    }
}