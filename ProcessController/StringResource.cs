using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessController
{
    public static class StringResource
    {
        public static readonly string appTitle = "Process Controller";
        public static readonly string mainWinTitle = appTitle + "";
        public static readonly string passwordApplyWinTitle = appTitle + " - 비밀번호 변경";
        public static readonly string passwordInputWinTitle = appTitle + " - 계정 확인";
        public static readonly string notifyMenuTitle = appTitle + "";

        public static readonly string notifyMenuItem0 = "미지정 기능";
        public static readonly string notifyMenuItem1 = "프로그램 종료";

        public static readonly string passwordFileName = "password.pass";
        public static readonly string bootOptionFileName = "bootStart.option";
        public static readonly string blockProcessFileName = "blockProcess.list";


        public static readonly List<string> blockProcessesName = new List<string>()
        {
                "chrome",
                "HancomStudio",
                "Hwp",
                "HCell",
                "HShow",
                "Hpdf",
                "MicrosoftEdge",
                "Taskmgr"
        };
    }
}
