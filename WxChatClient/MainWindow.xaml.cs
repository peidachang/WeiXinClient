namespace WxChatClient
{
    using FirstFloor.ModernUI.Windows.Controls;
    using System.Windows;
    using WxChatClient.Common;

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        public MainWindowModel ViewModel { get; set; }

        public MainWindow()
        {
            ViewModel = new MainWindowModel();
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WebChat chat = new WebChat();
            MessageBox.Show(chat.GetUserInfo("109147935"));
        }
    }
}
