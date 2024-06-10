using System;
using Gtk;
using MediaTekDocuments.Dal;
using MediaTekDocuments.Model;
using UI = Gtk.Builder.ObjectAttribute;

namespace MediaTekDocuments.View
{
    public class CreateDvdDialog : Dialog
    {
        [UI]
        private Entry _inputTitle;
        [UI]
        private Entry _inputDirector;
        [UI]
        private SpinButton _inputDuration;
        [UI]
        private TextView _inputSynopsis;
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

        public CreateDvdDialog(Program program) : this(program, new Builder("CreateDvdDialog.glade")) { }

        private CreateDvdDialog(Program program, Builder builder) : base(builder.GetRawOwnedObject("CreateDvdDialog"))
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
            Dvd dvd = new(null,
                this._inputTitle.Text,
                null,
                this._inputDuration.ValueAsInt,
                this._inputDirector.Text,
                this._inputSynopsis.Buffer.Text,
                this._comboGenre.ActiveId,
                null,
                this._comboPublic.ActiveId,
                null,
                this._comboAisle.ActiveId,
                null);

            await Access.GetInstance().Dvds().Create(dvd);

            this.Destroy();
        }
    }
}