﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProcessController
{
    /// <summary>
    /// Window1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PasswordInput : Window
    {
        public static PasswordInput GetPasswordInputWin
        {
            get
            {
                if (App.PasswordInputWin is null)
                {
                    App.PasswordInputWin = new PasswordInput();
                }
                return App.PasswordInputWin;
            }

        }
        private PasswordInput()
        {
            InitializeComponent();
            this.Title = StringResource.passwordInputWinTitle;
            exitButton.Visibility = Visibility.Hidden;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (App.LoginSession)
            {
                passwordInputText.Password = "";
                WindowEventHandler.WindowHidingClose(this,sender,e);
            }
            else
            {
                System.Environment.Exit(0);
            }
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            if (passwordInputText.Password == App.Password)
            {
                if (App.LoginSession == false)
                {
                    PasswordInputLoginAct();
                }
                else
                {
                    this.Hide();
                    App.MainWin.Show();
                }
                passwordInputText.Password = "";
            }
            else if (passwordInputText.Password == App.emergencyExitKey)
            {
                MessageBox.Show("개발자 전용 긴급 exit 백도어");
                System.Environment.Exit(0);
            }
            else
            {
                passwordInputText.Password = "";
                MessageBox.Show("비밀번호가 틀렸습니다.");
            }
        }
        public void PasswordInputLoginAct()
        {
            App.LoginSession = true;
            exitButton.Visibility = Visibility.Visible;
            statusText.Text = "현재 프로그램이 동작중입니다.";
            App.MainWin = MainWindow.GetMainWin;
            App.MainWin.Show();
            this.Hide();
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            if (passwordInputText.Password == App.Password)
            {
                System.Environment.Exit(0);
            }
            else
            {
                MessageBox.Show("비밀번호가 틀렸습니다.");
            }
        }
    }
}
