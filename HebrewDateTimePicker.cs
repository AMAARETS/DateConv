using System;                                                 // שימושבספריית System  
using System.Drawing;                                         // שימושבספריית System.Drawing לציור  
using System.Runtime.InteropServices;                        // ייבוא DllImport ו־Marshal  
using System.Windows.Forms;                                   // שימושב־WinForms  

// מבנה הכותרת של הודעות Notify
[StructLayout(LayoutKind.Sequential)]
internal struct NMHDR
{
    public IntPtr hwndFrom;                                    // חלון השולח  
    public IntPtr idFrom;                                      // מזהה הבקר  
    public int code;                                           // קוד ההודעה  
}

// מבנה למידע ציור מותאם
[StructLayout(LayoutKind.Sequential)]
internal struct NMCUSTOMDRAW
{
    public NMHDR hdr;                                          // הכותרת  
    public int dwDrawStage;                                    // שלב הציור  
    public IntPtr hdc;                                         // Context לציור  
    public RECT rc;                                            // אזור הציור  
    public IntPtr dwItemSpec;                                  // זיהוי הפריט  
    public int uItemState;                                     // מצב הפריט (פעיל/כבוי)  
    public IntPtr lItemlParam;                                 // פרמטר נוסף  
}

// מבנה מלבן לשימוש ב־NMCUSTOMDRAW
[StructLayout(LayoutKind.Sequential)]
internal struct RECT
{
    public int left, top, right, bottom;                       // קואורדינטות הריבוע  
}

// מחלקת תת-חלון לציור מותאם
internal class MonthCalendarSubclass : NativeWindow            // יורשת מ־NativeWindow כדי לתפוס הודעות
{
    private DateTimePicker picker;                             // הפקד הראשי 

    public MonthCalendarSubclass(IntPtr handle, DateTimePicker parent)  // קונסטרקטור
    {
        picker = parent;                                       // שמירת הפקד הראשי
        AssignHandle(handle);                                  // הקצאת ה־HWND לתת-חלון
    }

    protected override void WndProc(ref Message m)
    {
        const int WM_NOTIFY = 0x004E;                          // הודעת Notify כללית
        const int NM_CUSTOMDRAW = -12;                         // קוד NM_CUSTOMDRAW
        const int CDDS_ITEMPREPAINT = 0x00000001;              // שלב לפני ציור פריט
        const int CDRF_SKIPDEFAULT = 0x00000004;               // דילוג על ציור ברירת מחדל
        const int CDIS_DISABLED = 0x00000002;                  // מצב כבוי

        if (m.Msg == WM_NOTIFY)                                 // אם התקבלה Notify
        {
            NMHDR nmhdr = (NMHDR)Marshal.PtrToStructure(m.LParam, typeof(NMHDR));  // פרשנות הכותרת
            if (nmhdr.code == NM_CUSTOMDRAW)                  // אם זו הודעת Custom Draw
            {
                NMCUSTOMDRAW cd = (NMCUSTOMDRAW)Marshal.PtrToStructure(m.LParam, typeof(NMCUSTOMDRAW));  // פרשנות מידע הציור
                if ((cd.dwDrawStage & CDDS_ITEMPREPAINT) != 0) // שלב ציור פריט
                {
                    // חישוב תאריך התא לפי מיקום הריבוע
                    DateTime cellDate = GetDateFromCell(cd.rc);  // פונקציה לדוגמה לחישוב תאריך :contentReference[oaicite:9]{index=9}

                    if (cellDate.Month == picker.Value.Month)   // אם בחודש הנוכחי
                    {
                        cd.uItemState &= ~CDIS_DISABLED;      // הפיכת התא לפעיל
                    }
                    else                                        // אם בתא משני
                    {
                        cd.uItemState |= CDIS_DISABLED;       // סימון התא כבוי
                    }

                    // ציור טקסט מותאם במקום המספר
                    using (Graphics g = Graphics.FromHdc(cd.hdc))  // יצירת Graphics מ־HDC
                    {
                        Rectangle rect = Rectangle.FromLTRB(cd.rc.left, cd.rc.top, cd.rc.right, cd.rc.bottom); // בניית מלבן
                        TextRenderer.DrawText(g, "טקסט שלי", picker.Font, rect, Color.Black, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter); // ציור טקסט
                    }

                    m.Result = (IntPtr)CDRF_SKIPDEFAULT;         // דילוג על ציור ברירת מחדל :contentReference[oaicite:10]{index=10}
                    return;                                       // יציאה ללא העברת ההודעה הלאה
                }
            }
        }

        base.WndProc(ref m);                                   // קריאה ברירת מחדל לעיבוד שאר ההודעות
    }

    private DateTime GetDateFromCell(RECT rc)                 // פונקציה לחישוב תאריך לפי מיקום התא
    {
        // כאן ניתן לממש חישוב על פי GridDimensions וגודל תא
        return picker.Value;                                  // בדוגמה: מחזירים תאריך נוכחי פשוט  
    }
}

// פקד ראשי היורש מ־DateTimePicker
public class HebrewDateTimePicker : DateTimePicker
{
    private MonthCalendarSubclass mcSubclass;                 // משתנה לאחסון תת-החלון

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam); // ייבוא SendMessage

    protected override void WndProc(ref Message m)
    {
        const int DTM_FIRST = 0x1000;                         // בסיס הודעות DateTimePicker
        const int DTM_GETMONTHCALENDAR = DTM_FIRST + 8;       // קוד לקבלת MonthCalendar

        if (m.Msg == DTM_GETMONTHCALENDAR)                    // אם מתקבלת ההודעה
        {
            IntPtr calHandle = SendMessage(this.Handle, DTM_GETMONTHCALENDAR, IntPtr.Zero, IntPtr.Zero); // בקשת ה־HWND
            if (mcSubclass == null)                           // אם לא נוצר עדיין תת-החלון
            {
                mcSubclass = new MonthCalendarSubclass(calHandle, this); // הקצאת תת-החלון
            }
        }

        base.WndProc(ref m);                                   // קריאה לברירת מחדל
    }
}
