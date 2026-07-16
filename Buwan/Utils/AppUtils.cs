using SDL3;
using System;
using System.Collections.Generic;
using System.Text;

namespace Buwan.Utils
{
    internal class AppUtils
    {
        public static void ShowErrorBox(string message)
        {
            SDL.ShowSimpleMessageBox(SDL.MessageBoxFlags.Error,
                                     "Error",
                                     message,
                                     IntPtr.Zero);

            SDL.Log($"Error: {message}");
        }
    }
}
