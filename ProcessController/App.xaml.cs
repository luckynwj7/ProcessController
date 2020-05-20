using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProcessController
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        private static MainWindow mainWin;
        public static MainWindow MainWin
        {
            get { return mainWin; }
            set { mainWin = value; }
        }
        private static PasswordApply passwordApplyWin;
        public static PasswordApply PasswordApplyWin
        {
            get { return passwordApplyWin; }
            set { passwordApplyWin = value; }
        }
        private static PasswordInput passwordInputWin;
        public static PasswordInput PasswordInputWin
        {
            get { return passwordInputWin; }
            set { passwordInputWin = value; }
        }
        private static string password;
        public static string Password
        {
            set { password = value; }
            get { return password; }
        }
        private static bool loginSession;
        public static bool LoginSession
        {
            get { return loginSession; }
            set { loginSession = value; }
        }
        private static string currentPasswordPath;
        public static string CurrentPasswordPath
        {
            get { return currentPasswordPath; }
        }

        private NotifyIconHandler myNotify;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // 노티파이 메뉴 만들기
            NotifyIconHandler.SetNotifyObj(2, "WPF", ProcessController.Properties.Resources.defaultIcon, NotifyMenuMethod.NotifyTitleMethod);
            myNotify = NotifyIconHandler.NotifyObj;
            NotifyMenuMethod.SetNotifyMenu(myNotify);

            currentPasswordPath = System.Windows.Forms.Application.StartupPath + "\\password.pass";
            // 비밀번호 파일 읽기 (없으면 새로 생성)
            if (System.IO.File.Exists(currentPasswordPath))
            {
                password = System.IO.File.ReadAllText(currentPasswordPath);
            }
            else
            {
                FileStream myFile = System.IO.File.Create(currentPasswordPath);
                myFile.Close();
                System.IO.File.WriteAllText(currentPasswordPath, "MasterPass"); // 초기 마스터 비밀번호 생성
                password = System.IO.File.ReadAllText(currentPasswordPath);
            }

            if (password == "MasterPass")
            {
                MessageBox.Show("첫 이용입니다. 비밀번호를 설정해주세요.");
                passwordApplyWin = PasswordApply.GetPasswordApplyWin;
                PasswordApplyWin.Show();

            }
            else
            {
                LoginSession = false;
                passwordInputWin = PasswordInput.GetPasswordInputWin;
                passwordInputWin.Show();
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Console.WriteLine("ㅎㅎㅎ");
        }

        private void Application_Deactivated(object sender, EventArgs e)
        {
            Console.WriteLine("ㅁㅁㅁ");
        }
    }

}
