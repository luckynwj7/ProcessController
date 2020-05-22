using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        private string currentBlockFileListPath;
        // 차단 목록을 읽어올 파일 리스트 경로
        private List<string> blockFileList = new List<string>();
        public List<string> BlockFileList
        {
            get { return this.blockFileList; }
        }
        // 차단 목록을 split하여 얻은 list
        private List<string> noneBlockFileList = new List<string>();
        public List<string> NoneFileList
        {
            get { return this.noneBlockFileList; }
        }
        // 차단 가능한 모든 프로세스 목록
        private List<string> finalBlockList = new List<string>();
        public List<string> FinalBlockList
        {
            get { return finalBlockList; }
            set { finalBlockList = value; }
        }


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
            noneBlockFileList = StringResource.blockProcessesName;
            noneBlockFileList.Sort();

            // 차단 프로세스 리스트 파일 읽기 (없으면 새로 생성)
            currentBlockFileListPath = System.Windows.Forms.Application.StartupPath + "\\" + StringResource.blockProcessFileName;
            if (System.IO.File.Exists(currentBlockFileListPath))
            {
                string prevPass = System.IO.File.ReadAllText(currentBlockFileListPath);
                if(prevPass != "")
                {
                    string tempBlockStr = PasswordEncryption.DecrypString(prevPass, App.passwordEncryptionKey);
                    string[] splitWords = tempBlockStr.Split(',');
                    foreach (string str in splitWords)
                    {
                        blockFileList.Add(str);
                        noneBlockFileList.Remove(str);
                    }
                    finalBlockList = blockFileList;
                    finalBlockList.Sort();
                }
            }
            else
            {
                FileStream myFile = System.IO.File.Create(currentBlockFileListPath);
                myFile.Close();
                System.IO.File.WriteAllText(currentBlockFileListPath, ""); //빈 파일 생성
            }
            ProcessManager.UpdateProcessList(blockFileList, noneBlockFileList, currentBlockProcessList, canBlockProcessList);
            foreach(string str in blockFileList)
            {
                Console.WriteLine("차단 : " + str);
            }
            foreach (string str in noneBlockFileList)
            {
                Console.WriteLine("비차단 : " + str);
            }


            // 부팅 옵션 처리
            if (App.AppBootOptionManager.AutoBootingOption == true)
            {
                bootingOptionCheck.IsChecked = true;
            }
            else
            {
                bootingOptionCheck.IsChecked = false;
            }

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

        private void moveProcessToRightButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessManager.ProcessListMove(blockFileList, noneBlockFileList, currentBlockProcessList);
            ProcessManager.UpdateProcessList(blockFileList, noneBlockFileList, currentBlockProcessList, canBlockProcessList);
        }

        private void moveProcessToLeftButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessManager.ProcessListMove(noneBlockFileList, blockFileList, canBlockProcessList);
            ProcessManager.UpdateProcessList(blockFileList, noneBlockFileList, currentBlockProcessList, canBlockProcessList);
        }

        private void processBlockApplyButton_Click(object sender, RoutedEventArgs e)
        {
            finalBlockList = blockFileList;
            finalBlockList.Sort();
            string inputEncrypText = "";
            foreach(string splitWord in finalBlockList)
            {
                inputEncrypText += (splitWord + ",");
            }
            if (inputEncrypText.Length > 1)
            {
                inputEncrypText = inputEncrypText.Remove(inputEncrypText.Length - 1);
                inputEncrypText = PasswordEncryption.EncrypString(inputEncrypText, App.passwordEncryptionKey);
            }
            else
            {
                inputEncrypText = "";
            }
            System.IO.File.WriteAllText(currentBlockFileListPath, inputEncrypText); // 암호화 하여 저장


            if (bootingOptionCheck.IsChecked == true)
            {
                App.AppBootOptionManager.ChangeBootOption(true);
            }
            else if (bootingOptionCheck.IsChecked == false)
            {
                App.AppBootOptionManager.ChangeBootOption(false);
            }
            MessageBox.Show("변경 사항이 저장되었습니다.");
        }

        private void mainWindowHidingButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        
    }
}
