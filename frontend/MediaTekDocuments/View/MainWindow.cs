using Gtk;
using MediaTekDocuments.dal;
using MediaTekDocuments.Model;
using UI = Gtk.Builder.ObjectAttribute;

namespace MediaTekDocuments.View
{
    public class MainWindow : ApplicationWindow
    {
        private readonly Program _program;

        [UI]
        private ComboBox _genreCombo;
        [UI]
        private TreeView _bookList;

        public MainWindow(Program program) : this(program, new Builder("MainWindow.glade")) { }

        private MainWindow(Program program, Builder builder) : base(builder.GetRawOwnedObject("MainWindow"))
        {
            this._program = program;
            this.Application = program.GetApplication();
            builder.Autoconnect(this);

            DeleteEvent += Window_DeleteEvent;

            this.FillData();
        }

        private async void FillData()
        {
            var genres = await Access.GetInstance().GetAllGenres();
            ListStore genresModel = new(GLib.GType.String, GLib.GType.String);

            foreach (Genre g in genres)
            {
                genresModel.AppendValues(g.Id, g.Libelle);
            }

            this._genreCombo.Model = genresModel;

            CellRendererText txtRender = new();
            this._genreCombo.PackStart(txtRender, true);
            this._genreCombo.SetAttributes(txtRender, "id", 0);
            this._genreCombo.AddAttribute(txtRender, "text", 1);

            this._genreCombo.IdColumn = 1;
            this._genreCombo.Active = 0;

            this._genreCombo.Sensitive = true;
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }
    }
}
