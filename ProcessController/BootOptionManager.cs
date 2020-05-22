using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessController
{
    public class BootOptionManager
    {
        // 부팅 옵션을 결정하는 변수
        private Microsoft.Win32.RegistryKey runRegKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        private string currentBootOptionFilePath;
        private bool autoBootingOption;
        public bool AutoBootingOption
        {
            get { return autoBootingOption; }
        }

        public BootOptionManager()
        {
            currentBootOptionFilePath = System.Windows.Forms.Application.StartupPath + "\\" + StringResource.bootOptionFileName;

            // 비밀번호 파일 읽기 (없으면 새로 생성)
            if (System.IO.File.Exists(currentBootOptionFilePath))
            {
                string prevPass = System.IO.File.ReadAllText(currentBootOptionFilePath);
                string resultStr = PasswordEncryption.DecrypString(prevPass, App.passwordEncryptionKey);
                if (resultStr.Equals("true"))
                {
                    autoBootingOption = true;
                    runRegKey.SetValue("ProcessContorller", Environment.CurrentDirectory + "\\" + AppDomain.CurrentDomain.FriendlyName);
                }
                else if (resultStr.Equals("false"))
                {
                    autoBootingOption = false;
                    runRegKey.SetValue("ProcessController", false);
                }
            }
            else
            {
                FileStream myFile = System.IO.File.Create(currentBootOptionFilePath);
                myFile.Close();
                string prevPass = PasswordEncryption.EncrypString("true", App.passwordEncryptionKey);
                System.IO.File.WriteAllText(currentBootOptionFilePath, prevPass); //true로 기본 파일 생성
                autoBootingOption = true;
                runRegKey.SetValue("ProcessController", true);
            }
        }
        public void ChangeBootOption(bool flag)
        {
            if (flag)
            {
                string prevPass = PasswordEncryption.EncrypString("true", App.passwordEncryptionKey);
                System.IO.File.WriteAllText(currentBootOptionFilePath, prevPass);
                autoBootingOption = true;
                runRegKey.SetValue("ProcessContorller", Environment.CurrentDirectory + "\\" + AppDomain.CurrentDomain.FriendlyName);
            }
            else
            {
                string prevPass = PasswordEncryption.EncrypString("false", App.passwordEncryptionKey);
                System.IO.File.WriteAllText(currentBootOptionFilePath, prevPass);
                autoBootingOption = false;
                runRegKey.SetValue("ProcessController", false);
            }
        }
    }
}
