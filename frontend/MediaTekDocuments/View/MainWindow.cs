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
        private readonly ComboBox _aisleCombo;
        [UI]
        private readonly ComboBox _publicCombo;
        [UI]
        private readonly ComboBox _genreCombo;
        [UI]
        private readonly TreeView _bookList;

        public MainWindow(Program program) : this(program, new Builder("MainWindow.glade")) { }

        private MainWindow(Program program, Builder builder) : base(builder.GetRawOwnedObject("MainWindow"))
        {
            this._program = program;
            this.Application = program.GetApplication();
            builder.Autoconnect(this);

            DeleteEvent += Window_DeleteEvent;

            this.FillAisles();
            this.FillPublics();
            this.FillGenres();
        }

        private async void FillAisles()
        {
            var aisles = await Access.GetInstance().GetAllRayons();
            ListStore aislesModel = new(GLib.GType.String, GLib.GType.String);
            aislesModel.AppendValues(null, null);

            foreach (Rayon r in aisles)
            {
                aislesModel.AppendValues(r.Id, r.Libelle);
            }

            this._aisleCombo.Model = aislesModel;

            SetComboboxTextRenderer(this._aisleCombo);
        }

        private async void FillPublics()
        {
            var publics = await Access.GetInstance().GetAllPublics();
            ListStore publicsModel = new(GLib.GType.String, GLib.GType.String);
            publicsModel.AppendValues(null, null);

            foreach (Public p in publics)
            {
                publicsModel.AppendValues(p.Id, p.Libelle);
            }

            this._publicCombo.Model = publicsModel;

            SetComboboxTextRenderer(this._publicCombo);
        }

        private async void FillGenres()
        {
            var genres = await Access.GetInstance().GetAllGenres();
            ListStore genresModel = new(GLib.GType.String, GLib.GType.String);
            genresModel.AppendValues(null, null);

            foreach (Genre g in genres)
            {
                genresModel.AppendValues(g.Id, g.Libelle);
            }

            this._genreCombo.Model = genresModel;

            SetComboboxTextRenderer(this._genreCombo);
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }

        private static void SetComboboxTextRenderer(ComboBox cbx)
        {
            CellRendererText txtRender = new();
            cbx.PackStart(txtRender, true);
            cbx.SetAttributes(txtRender, "id", 0);
            cbx.AddAttribute(txtRender, "text", 1);

            cbx.IdColumn = 1;
            cbx.Active = 0;

            cbx.Sensitive = true;
        }
    }
}
