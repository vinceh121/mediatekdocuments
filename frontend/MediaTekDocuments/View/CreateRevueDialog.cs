using System;
using Gtk;
using MediaTekDocuments.Dal;
using MediaTekDocuments.Model;
using UI = Gtk.Builder.ObjectAttribute;

namespace MediaTekDocuments.View
{
    public class CreateRevueDialog : Dialog
    {
        [UI]
        private Entry _inputTitle;
        [UI]
        private SpinButton _inputDelaiMiseADispo;
        [UI]
        private ComboBox _inputPeriodicite;
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

        public CreateRevueDialog(Program program) : this(program, new Builder("CreateRevueDialog.glade")) { }

        private CreateRevueDialog(Program program, Builder builder) : base(builder.GetRawOwnedObject("CreateRevueDialog"))
        {
            this.Application = program.GetApplication();
            builder.Autoconnect(this);

            Utils.SetComboboxTextRenderer(this._inputPeriodicite);

            this._btnCancel.Clicked += (_, _) => this.Destroy();
            this._btnCreate.Clicked += (_, _) => this.CreateRevue();

            Utils.FillEnum(Access.GetInstance().Aisles(), this._comboAisle);
            Utils.FillEnum(Access.GetInstance().Publics(), this._comboPublic);
            Utils.FillEnum(Access.GetInstance().Genres(), this._comboGenre);
        }

        private async void CreateRevue()
        {
            Revue r = new(null,
                this._inputTitle.Text,
                null,
                this._comboGenre.ActiveId,
                null,
                this._comboPublic.ActiveId,
                null,
                this._comboAisle.ActiveId,
                null,
                this._inputPeriodicite.ActiveId,
                this._inputDelaiMiseADispo.ValueAsInt);

            await Access.GetInstance().Revues().Create(r);

            this.Destroy();
        }
    }
}