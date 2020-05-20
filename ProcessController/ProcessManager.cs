using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProcessController
{
    public static class ProcessManager
    {
        public static void ProcessRead()
        {

            Process[] processes = Process.GetProcesses();
            App.MainWin.processList.Text = "";
            foreach (Process pro in processes)
            {
                string processName = pro.ToString() + "\n";
                App.MainWin.processList.Text += processName;
            }

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
                MessageBox.Show(blockProcessName+" 프로세스 실행 차단");
            }
        }

        public static void ProcessesKill()
        {
            // 차단할 프로세스 모음
            string[] blockProcessesName =
            {
                //"chrome",
                "HancomStudio",
                "Hwp",
                "HCell",
                "HShow",
                "Hpdf",
                "MicrosoftEdge"
            };

            // 프로세스 하나씩 차단
            foreach (string proName in blockProcessesName)
            {
                ProcessKill(proName);
            }
            
        }
    }
}
