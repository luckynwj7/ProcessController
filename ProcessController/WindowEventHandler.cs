using System;
using System.ComponentModel;
using System.Windows;

namespace ProcessController
{
    public static class WindowEventHandler
    {
        public static void WindowHidingClose(Window myWin, object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            myWin.Hide();
        }
    }

}