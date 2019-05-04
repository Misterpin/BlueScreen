using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blue_Screen
{
    public class NativeMethods
    {
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "BlockInput")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool BlockInput([System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)] bool fBlockIt);

        public static void BlockInputTemp(int millis)
        {
            try {
                BlockInput(true);
                Thread.Sleep(millis);
            } finally {
                BlockInput(false);
            }
        }

        public static bool SetAutorunValue(bool autorun)
        {
            string ExePath = System.Windows.Forms.Application.ExecutablePath;
            RegistryKey reg = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");
            try
            {
                if (autorun)
                    reg.SetValue("Blue Screen", ExePath);
                else
                    reg.DeleteValue("Blue Screen");

                reg.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
