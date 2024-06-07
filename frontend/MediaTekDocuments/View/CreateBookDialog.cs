using System;
using Gtk;
using MediaTekDocuments.Dal;
using MediaTekDocuments.Model;
using UI = Gtk.Builder.ObjectAttribute;

namespace MediaTekDocuments.View
{
    public class CreateBookDialog : Dialog
    {
        [UI]
        private Entry _inputTitle;
        [UI]
        private Entry _inputAuthor;
        [UI]
        private Entry _inputIsbn;
        [UI]
        private Entry _inputCollection;
        [UI]
        private ComboBox _comboAisle;
        [UI]
        private ComboBox _comboPublic;
        [UI]
        private ComboBox _comboGenre;

        [UI]
        private Button _btnCreate;
        [UI]
        private Button _btnCancel;

        public CreateBookDialog(Program program) : this(program, new Builder("CreateBookDialog.glade")) { }

        private CreateBookDialog(Program program, Builder builder) : base(builder.GetRawOwnedObject("CreateBookDialog"))
        {
            this.Application = program.GetApplication();
            builder.Autoconnect(this);

            this._btnCancel.Clicked += (_, _) => Application.Quit();
            this._btnCreate.Clicked += (_, _) => this.CreateBook();

            this.FillAisles();
            this.FillPublics();
            this.FillGenres();
        }

        private async void CreateBook()
        {
            Livre l = new(null,
                this._inputTitle.Text,
                null,
                this._inputIsbn.Text,
                this._inputAuthor.Text,
                this._inputCollection.Text,
                this._comboGenre.ActiveId,
                null,
                this._comboPublic.ActiveId,
                null,
                this._comboAisle.ActiveId,
                null);

            await Access.GetInstance().CreateBook(l);

            this.Destroy();
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

            this._comboAisle.Model = aislesModel;

            MainWindow.SetComboboxTextRenderer(this._comboAisle);
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

            this._comboPublic.Model = publicsModel;

            MainWindow.SetComboboxTextRenderer(this._comboPublic);
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

            this._comboGenre.Model = genresModel;

            MainWindow.SetComboboxTextRenderer(this._comboGenre);
        }
    }
}