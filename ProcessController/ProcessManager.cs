using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;

namespace ProcessController
{
    public static class ProcessManager
    {
        private static List<string> blockProcessName = new List<string>();
        public static List<string> BlockProcessName
        {
            get { return blockProcessName; }
        }
        private static void ProcessKill(string blockProcessName)
        {
            Process[] myProcesses = Process.GetProcessesByName(blockProcessName);
            if (myProcesses.Length > 0)
            {
                foreach(Process killPro in myProcesses)
                {
                    killPro.Kill();
                }
                if(blockProcessName.Equals("Taskmgr")) { blockProcessName = "작업 관리자"; }
                MessageBox.Show(blockProcessName+" 프로세스 실행 차단");
            }
        }

        public static void ProcessesKill()
        {
            // 프로세스 하나씩 차단
            foreach (string proName in App.MainWin.FinalBlockList)
            {
                ProcessKill(proName);
            }
            
        }

        private static void ShowProcessList(List<string> showList, List<string> noneShowList, StackPanel listBox)
        {
            // 프로세스 리스트와 논리스트가 서로 일치하지 않으면 추가
            foreach (string showName in showList)
            {
                bool overlapFlag = false;
                foreach (string noneName in noneShowList)
                {
                    
                    if (noneName.Equals(showName))
                    {
                        overlapFlag = true;
                        break;
                    }
                }
                if (overlapFlag)
                {
                    overlapFlag = false;
                }
                else
                {
                    listBox.Children.Add(new ToggleButton());
                    (listBox.Children[listBox.Children.Count - 1] as ToggleButton).Background = Brushes.White;
                    (listBox.Children[listBox.Children.Count - 1] as ToggleButton).Content = showName;
                    (listBox.Children[listBox.Children.Count - 1] as ToggleButton).Height = 30;
                }
            }
        }
        public static void UpdateProcessList(List<string> blockList, List<string> currentList, StackPanel blockListBox, StackPanel currentListBox)
        {
            blockListBox.Children.RemoveRange(0, blockListBox.Children.Count);
            currentListBox.Children.RemoveRange(0, currentListBox.Children.Count);
            ShowProcessList(blockList, currentList, blockListBox);
            ShowProcessList(currentList, blockList, currentListBox);
        }

        public static void ProcessListMove(List<string> inFileList, List<string> outFileList, StackPanel inFileSt)
        {
            foreach (ToggleButton process in inFileSt.Children)
            {
                if (process.IsChecked == true)
                {
                    string addedProcess = process.Content as string;
                    inFileList.Remove(addedProcess);
                    outFileList.Add(addedProcess);
                }
            }
        }
        
        public static void ReadBlockProcessName()
        {
            blockProcessName = null;
            blockProcessName = new List<string>();
            string filePath = System.Windows.Forms.Application.StartupPath + "\\" + StringResource.canBlockProcessFileName;
            if (System.IO.File.Exists(filePath))
            {
                string prevPass = System.IO.File.ReadAllText(filePath);
                if (prevPass != "")
                {
                    string[] words = prevPass.Split(new string[] { "\r\n", "\n"}, StringSplitOptions.None );
                    foreach(string str in words)
                    {
                        blockProcessName.Add(str);
                        Console.WriteLine("추가됨 : " + str);
                    }
                }
            }
            else
            {
                FileStream myFile = System.IO.File.Create(filePath);
            }
        }
    }
}
