using System;
using Gtk;
using MediaTekDocuments.Dal;
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
            try
            {
                var res = await Access.GetInstance().Login(this._inputEmail.Text, this._inputPwd.Text);

                var win = new MainWindow(this._program, res.ReadOnly);
                win.ShowAll();
                this.Destroy();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                MessageDialog diag = new(null,
                                    0,
                                    MessageType.Error,
                                    ButtonsType.Ok,
                                    false,
                                    "Error de connexion: {0}",
                                    [e.Message]);

                diag.Run();
                diag.Destroy();
            }
        }
    }
}