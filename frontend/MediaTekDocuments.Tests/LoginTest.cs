using System;
using Gtk;
using MediaTekDocuments.View;
using Xunit;

namespace MediaTekDocuments.Tests
{
	public class LoginTest
	{
		private Program _program;
		private LoginDialog _login;

		public LoginTest()
		{
			this._program = new();
			this._login = new(this._program);
		}

		[Fact]
		public void TestLogin()
		{
			var email = TestUtils.GetChildByName<Entry>(this._login, "_inputEmail");
			Assert.NotNull(email);
			
			var password = TestUtils.GetChildByName<Entry>(this._login, "_inputPwd");
			Assert.NotNull(password);

			email.Text = "admin@org";
			password.Text = "A15T";

			var login = TestUtils.GetChildByName<Button>(this._login, "_btnLogin");
			login.Click();
		}
	}
}