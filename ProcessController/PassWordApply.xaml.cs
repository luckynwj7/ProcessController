using System;
using System.Collections.Generic;
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
    public partial class PasswordApply : Window
    {
        public static PasswordApply GetPasswordApplyWin
        {
            get
            {
                if (App.PasswordApplyWin is null)
                {
                    App.PasswordApplyWin = new PasswordApply();
                }
                return App.PasswordApplyWin;
            }

        }
        private PasswordApply()
        {
            InitializeComponent();
        }

        private void completeButton_Click(object sender, RoutedEventArgs e)
        {
            string prevPass = PasswordEncryption.EncrypString(passwordInputText.Text, App.passwordEncryptionKey);
            System.IO.File.WriteAllText(App.CurrentPasswordPath, prevPass);
            App.Password = passwordInputText.Text;
            passwordInputText.Text = "";
            MessageBox.Show("비밀번호 변경이 완료되었습니다.");
            if (!App.LoginSession)
            {
                App.PasswordInputWin = PasswordInput.GetPasswordInputWin;
                App.PasswordInputWin.PasswordInputLoginAct();
            }
            this.Close();
        }
    }
}
