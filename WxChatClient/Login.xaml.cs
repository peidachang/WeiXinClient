using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
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
using WxChatClient.Common;

namespace WxChatClient
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        private TaskScheduler _default;

        public LoginModel ViewModel { get; set; }

        public Login()
        {
            this.ViewModel = new LoginModel();

            InitializeComponent();

            this._default = TaskScheduler.FromCurrentSynchronizationContext();
        }

        private void StartLogin()
        {
            WebChat chat = new WebChat();
            chat.Login(Global.ACCOUNT, Global.PASSWORD);
        }

        private void ShowMainWindow()
        {
            MainWindow main = new MainWindow();
            this.Close();
            main.Show();
            main.Activate();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Global.ACCOUNT = this.ViewModel.Account;
            Global.PASSWORD = this.txtPwd.Password;

            using (var md5 = MD5.Create())
            {
                var sb = new StringBuilder();
                var buffer = md5.ComputeHash(Encoding.UTF8.GetBytes(Global.PASSWORD));
                foreach (var b in buffer)
                {
                    sb.Append(b.ToString("x2"));
                }

                Global.PASSWORD = sb.ToString();
            }

            var btn = (Button)sender;
            btn.IsEnabled = false;

            Task.Factory.StartNew(StartLogin)
                .ContinueWith((task) =>
                {
                    btn.IsEnabled = true;
                    if (task.Exception != null)
                        MessageBox.Show("登录失败");
                    else
                        ShowMainWindow();
                }, _default);
        }
    }
}
