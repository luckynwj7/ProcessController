using System;

namespace ProcessController
{
    public static class NotifyMenuMethod
    {
        public static void SetNotifyMenu(NotifyIconHandler notifyObj)
        {
            notifyObj.SetNotifyItemFunc(0, StringResource.notifyMenuItem0, NotifyMethod1);
            notifyObj.SetNotifyItemFunc(1, StringResource.notifyMenuItem1, NotifyMethod2);
        }


        public static void NotifyTitleMethod()
        {
            if (App.MainWin is null || App.PasswordInputWin is null)
            {
                System.Environment.Exit(0);
            }

            if (!(App.MainWin is null))
            {
                App.MainWin.Hide();
            }
            if (App.PasswordInputWin.ShowInTaskbar)
            {
                App.PasswordInputWin.Hide();
            }
            App.PasswordInputWin.ShowDialog();
        }
        public static void NotifyMethod1()
        {
         
        }

        public static void NotifyMethod2()
        {
            if(App.MainWin is null || App.PasswordInputWin is null)
            {
                System.Environment.Exit(0);
            }
            else if (App.MainWin.ShowInTaskbar)
            {
                App.MainWin.Hide();
            }
            else if (App.PasswordInputWin.ShowInTaskbar)
            {
                App.PasswordInputWin.Hide();
            }
            App.PasswordInputWin.ShowDialog();
        }
    }
}