using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Documents;
using System.Windows.Forms;

namespace ProcessController 
{
    public class NotifyIconHandler
    {
        private System.Windows.Forms.ContextMenu menu;
        private NotifyIcon ni;

        private static NotifyIconHandler notifyObj;
        public static NotifyIconHandler NotifyObj
        {
            get { return notifyObj; }
        }
        public static void SetNotifyObj(int itemCount, string inputText, System.Drawing.Icon icon, System.Action actMethod)
        {
            notifyObj = new NotifyIconHandler(itemCount, inputText, icon, actMethod);
            
        }
        private NotifyIconHandler(int itemCount, string inputText, System.Drawing.Icon icon, System.Action actMethod)
        {
            this.menu = new System.Windows.Forms.ContextMenu(); // Menu 객체
            this.ni = new NotifyIcon();
            for (int count = 0; count < itemCount; count++)
            {
                System.Windows.Forms.MenuItem item = new System.Windows.Forms.MenuItem();
                item.Index = count;
                menu.MenuItems.Add(item);
            }
            this.ni.Icon = icon;
            this.ni.Visible = true;
            this.ni.DoubleClick += delegate (object senders, EventArgs args)    // Tray icon의 더블 클릭 이벤트 등록
            {
                actMethod();
            };
            this.ni.ContextMenu = this.menu;
            this.ni.Text = inputText;
        }

        public void SetNotifyItemFunc(int itemIndex,string inputText,System.Action actMethod)
        {
            this.menu.MenuItems[itemIndex].Text = inputText;
            this.menu.MenuItems[itemIndex].Click += delegate (object click, EventArgs eClick)    // menu 의 클릭 이벤트 등록
            {
                actMethod();
            };
        }

    }
}