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

            Utils.FillEnum(Access.GetInstance().Aisles(), this._comboAisle);
            Utils.FillEnum(Access.GetInstance().Publics(), this._comboPublic);
            Utils.FillEnum(Access.GetInstance().Genres(), this._comboGenre);
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

            await Access.GetInstance().Books().Create(l);

            this.Destroy();
        }
    }
}