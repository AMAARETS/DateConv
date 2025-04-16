using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DateConv
{

    public class SyncListBox : ListBox
    {
        public SyncListBox()
        {
            // אל תטרגו חריגות, ואל תנסו לגשת למשאבים חיצוניים בזמן עיצוב!
        }
        // הודעות גלילה
        private const int WM_HSCROLL = 0x0114; // גלילה אופקית :contentReference[oaicite:15]{index=15}
        private const int WM_VSCROLL = 0x0115; // גלילה אנכית :contentReference[oaicite:16]{index=16}
        private const int WM_MOUSEWHEEL = 0x020A; // גלילה אנכית בגלגל :contentReference[oaicite:17]{index=17}
        private const int WM_MOUSEHWHEEL = 0x020E; // גלילה אופקית בגלגל :contentReference[oaicite:18]{index=18}
        private const int WM_KEYDOWN = 0x0100; // לחיצה על מקש :contentReference[oaicite:19]{index=19}
        private const int WM_KEYUP = 0x0101; // שחרור מקש

        // קודי מקשים
        private const int VK_UP = 0x26;
        private const int VK_DOWN = 0x28;
        private const int VK_PRIOR = 0x21;
        private const int VK_NEXT = 0x22;
        private const int VK_HOME = 0x24;
        private const int VK_END = 0x23;

        // אירוע שמתרחש בכל גלילה
        public event ScrollEventHandler Scrolled;

        // דריסת WndProc כדי לתפוס הודעות גלילה :contentReference[oaicite:3]{index=3}
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);  // נותן למערכת לטפל קודם בהודעה

            bool isScrollMsg =
                m.Msg == WM_HSCROLL ||
                m.Msg == WM_VSCROLL ||
                m.Msg == WM_MOUSEWHEEL ||
                m.Msg == WM_MOUSEHWHEEL;

            bool isKeyScroll = false;
            if (m.Msg == WM_KEYDOWN)
            {
                // בדיקה של כל מקשי הגלילה במקלדת
                int vk = m.WParam.ToInt32();
                isKeyScroll =
                    vk == VK_DOWN || // חץ למטה
                    vk == VK_UP || // חץ למעלה
                    vk == VK_PRIOR || // Page Up
                    vk == VK_NEXT || // Page Down
                    vk == VK_HOME || // Home
                    vk == VK_END;      // End
            }

            if (isScrollMsg || isKeyScroll)
            {
                // הפעלת האירוע עם המיקום החדש (TopIndex) של הפריט הראשון הנראה
                Scrolled?.Invoke(this, new ScrollEventArgs(
                    ScrollEventType.EndScroll, this.TopIndex));
            }
        }
    }
}
