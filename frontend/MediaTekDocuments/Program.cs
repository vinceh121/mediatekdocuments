using MediaTekDocuments.view;
using System;
using Gtk;

namespace MediaTekDocuments
{
	class Program
	{
		private Application _app;

		/// <summary>
		/// Point d'entrée principal de l'application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Program prog = new();
		}

		public Program()
		{
			Application.Init();

            this._app = new Application("me.vinceh121.mediatekdocuments", GLib.ApplicationFlags.None);
            this._app.Register(GLib.Cancellable.Current);
		}
	}
}
