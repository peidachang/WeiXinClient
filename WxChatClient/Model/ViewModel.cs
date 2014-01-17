namespace WxChatClient
{
	using System;
	using System.ComponentModel;
	using System.Xml;

	public partial class LoginModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private string _Account;
		public string Account
		{
			get { return _Account; }
			set 
			{				
				_Account = value;
				if(PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("Account"));
			}
		}

		private string _Password;
		public string Password
		{
			get { return _Password; }
			set 
			{				
				_Password = value;
				if(PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("Password"));
			}
		}
	}

	public partial class MainWindowModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
	}

}