using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
        public static readonly string passwordEncryptionKey = "R+eBzV276RIMdqaijDID6g==";
        //public static readonly string masterPassKey = "OGdkxWFBMYLK7hx6AtVM3rop/+eia0YKaet6S2zoskM="; // ==> "MasterPass"
        public static readonly string emergencyExitKey = "iwillexitthisapp";
        public static readonly string blockPassKey = "BlockPass"; // ==> "BlockPass"

        private static BootOptionManager appBootOptionManager;
        public static BootOptionManager AppBootOptionManager
        {
            get { return appBootOptionManager; }
        }

        private NotifyIconHandler myNotify;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //중복 실행 방지
            try
            {
                Process[] myProcesses = Process.GetProcessesByName("ProcessController");
                if (myProcesses.Length > 1){
                    MessageBox.Show("프로그램이 이미 실행중입니다");
                    System.Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {

            }

            // 노티파이 메뉴 만들기
            NotifyIconHandler.SetNotifyObj(2, StringResource.notifyMenuTitle, ProcessController.Properties.Resources.notifyIcon, NotifyMenuMethod.NotifyTitleMethod);
            myNotify = NotifyIconHandler.NotifyObj;
            NotifyMenuMethod.SetNotifyMenu(myNotify);

            // 부팅 옵션 처리
            appBootOptionManager = new BootOptionManager();

            // 블록 프로세스 읽기
            ProcessManager.ReadBlockProcessName();
            
            currentPasswordPath = System.Windows.Forms.Application.StartupPath + "\\" + StringResource.passwordFileName;

            // 비밀번호 파일 읽기
            if (System.IO.File.Exists(currentPasswordPath))
            {
                string prevPass = System.IO.File.ReadAllText(currentPasswordPath);
                if(prevPass != "")
                {
                    password = PasswordEncryption.DecrypString(prevPass, passwordEncryptionKey);
                }
                else
                {
                    password = "";
                }
            }
            else
            {
                /*
                FileStream myFile = System.IO.File.Create(currentPasswordPath);
                myFile.Close();
                System.IO.File.WriteAllText(currentPasswordPath, blockPassKey); //차단 파일 생성
                password = PasswordEncryption.DecrypString(blockPassKey, passwordEncryptionKey);*/
                MessageBox.Show("암호 파일에 직접 접근한 흔적이 있습니다. 마스터 암호로 실행해주세요.");
                FileStream myFile = System.IO.File.Create(currentPasswordPath);
                myFile.Close();
                string prevPass = PasswordEncryption.EncrypString(blockPassKey, passwordEncryptionKey);
                System.IO.File.WriteAllText(currentPasswordPath, prevPass); //차단 파일 생성
                password = blockPassKey;
                AutoLogin();
            }


            if (password == "")
            {
                MessageBox.Show("암호 파일에 직접 접근한 흔적이 있습니다. 마스터 암호로 실행해주세요.");
                string prevPass = PasswordEncryption.EncrypString(blockPassKey, passwordEncryptionKey);
                System.IO.File.WriteAllText(currentPasswordPath, prevPass); //차단 파일 생성
                password = blockPassKey;
                AutoLogin();
            }
            else if (password == "MasterPass") // 초기 마스터 비밀번호 생성코드 필요
            {
                MessageBox.Show("첫 이용입니다. 비밀번호를 설정해주세요.");
                passwordApplyWin = PasswordApply.GetPasswordApplyWin;
                PasswordApplyWin.Show();

            }
            else if (appBootOptionManager.AutoBootingOption)
            {
                // 자동 로그인
                AutoLogin();
                MessageBox.Show(StringResource.appTitle + "가 실행됩니다.");
            }
            else if(password == emergencyExitKey)
            {
                MessageBox.Show("개발자 전용 긴급 exit 백도어");
                System.Environment.Exit(0);
            }
            else
            {
                LoginSession = false;
                passwordInputWin = PasswordInput.GetPasswordInputWin;
                passwordInputWin.Show();
            }
        }

        private void AutoLogin()
        {
            LoginSession = true;
            passwordInputWin = PasswordInput.GetPasswordInputWin;
            PasswordInputWin.PasswordInputLoginAct();
            MainWin.Hide();
        }
    }


}