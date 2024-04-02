using Gtk;
using MediaTekDocuments.dal;
using UI = Gtk.Builder.ObjectAttribute;

namespace MediaTekDocuments.View
{
	public class LoginDialog : Dialog
	{
		private Program _program;

		[UI]
		private Entry _inputEmail;
		[UI]
		private Entry _inputPwd;

		[UI]
		private Button _btnCancel;
		[UI]
		private Button _btnLogin;

		public LoginDialog(Program program) : this(program, new Builder("LoginDialog.glade")) { }

		private LoginDialog(Program program, Builder builder) : base(builder.GetRawOwnedObject("LoginDialog"))
		{
			this._program = program;
			this.Application = program.GetApplication();
			builder.Autoconnect(this);

			this._btnCancel.Clicked += (_, _) => Application.Quit();
			this._btnLogin.Clicked += (_, _) => this.Login();
		}

		private async void Login()
		{
			var success = await Access.GetInstance().Login(this._inputEmail.Text, this._inputPwd.Text);

			if (success)
			{
				var win = new MainWindow(this._program);
				win.ShowAll();
				this.Destroy();
			}
			else
			{
				MessageDialog diag = new(null,
					0,
					MessageType.Error,
					ButtonsType.Ok,
					false,
					"Email ou Mot de passe invalide",
					[]);

				diag.Run();
				diag.Destroy();
			}
		}
	}
}