using Gtk;
using UI = Gtk.Builder.ObjectAttribute;

namespace MediaTekDocuments.View
{
	public class MainWindow : ApplicationWindow
	{
		private Program _program;

		public MainWindow(Program program) : this(program, new Builder("MainWindow.glade")) { }

		private MainWindow(Program program, Builder builder) : base(builder.GetRawOwnedObject("MainWindow"))
		{
			this._program = program;
			this.Application = program.GetApplication();
			builder.Autoconnect(this);
		}
	}
}
