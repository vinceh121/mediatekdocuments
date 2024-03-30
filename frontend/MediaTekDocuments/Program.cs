using MediaTekDocuments.View;
using System;
using Gtk;

namespace MediaTekDocuments
{
	public class Program
	{
		private readonly Application _app;
		private readonly MainWindow _mainWindow;

		/// <summary>
		/// Point d'entrée principal de l'application.
		/// </summary>
		[STAThread]
		public static void Main()
		{
			GLib.ExceptionManager.UnhandledException += HandleUncaughtException;

			Program prog = new();
			prog.Start();
		}

		public Program()
		{
			Application.Init();

			this._app = new Application("me.vinceh121.mediatekdocuments", GLib.ApplicationFlags.None);
			this._app.Register(GLib.Cancellable.Current);

			this._mainWindow = new(this);
			this._app.AddWindow(this._mainWindow);
		}

		public void Start()
		{
			this._mainWindow.ShowAll();
			Application.Run();
		}

		public Application GetApplication()
		{
			return this._app;
		}

		private static void HandleUncaughtException(GLib.UnhandledExceptionArgs args)
		{
			Console.WriteLine(args.ExceptionObject);
			MessageDialog diag = new(null, 0,
				MessageType.Error, ButtonsType.Ok, false,
				"Erreur inattendue : {0}", [args.ExceptionObject]);
			diag.Run();
			diag.Destroy();
		}
	}
}
