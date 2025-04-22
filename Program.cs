using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DateConv
{
    internal static class Program
    {
        private const int HOTKEY_ID = 1;
        // הגדרת המודיפייר: כאן Ctrl + Shift
        private const uint MOD_CONTROL = 0x0002;
        private const uint MOD_SHIFT = 0x0004;
        // קוד המקשים: כאן F12 (שימוש לדוגמה)
        private const uint VK_F12 = 0x7B;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (var form = new Form1())                    // יצירת מופע של הטופס הראשי
            {
                // רישום הקיצור גלובלי עם Ctrl+Shift+F12
                RegisterHotKey(form.Handle, HOTKEY_ID, MOD_CONTROL | MOD_SHIFT, VK_F12);
                Application.Run(form);                            // הרצת הלולאה הראשית של האפליקציה
                // ביטול רישום הקיצור בסיום
                UnregisterHotKey(form.Handle, HOTKEY_ID);
            }
        }
    }
}
