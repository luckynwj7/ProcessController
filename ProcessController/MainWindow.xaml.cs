using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MessageBox = System.Windows.Forms.MessageBox;

namespace ProcessController
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow GetMainWin
        {
            get
            {
                if (App.MainWin is null)
                {
                    App.MainWin = new MainWindow();
                }
                return App.MainWin;
            }

        }
        private MainWindow()
        {
            InitializeComponent();
            this.Title = StringResource.mainWinTitle;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            WindowEventHandler.WindowHidingClose(this, sender, e);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RealTimeEventHandler.RealTimeMethod(sender, e);
        }

        private void passwordEditButton_Click(object sender, RoutedEventArgs e)
        {
            App.PasswordApplyWin = PasswordApply.GetPasswordApplyWin;
            App.PasswordApplyWin.Show();
        }

        private void appExitButton_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(0);
        }
    }
}
