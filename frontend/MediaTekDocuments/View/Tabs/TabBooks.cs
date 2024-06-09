using System.Collections.Generic;
using Gtk;
using MediaTekDocuments.Dal;
using MediaTekDocuments.Model;
using UI = Gtk.Builder.ObjectAttribute;

namespace MediaTekDocuments.View.Tabs
{
    public class TabBooks : Container
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

        [UI]
        private readonly Entry _bookId;
        [UI]
        private readonly Entry _bookTitle;
        [UI]
        private readonly Entry _bookAuthor;
        [UI]
        private readonly Entry _bookCollection;
        [UI]
        private readonly Entry _bookImagePath;
        [UI]
        private readonly Entry _bookIsbn;
        [UI]
        private readonly ComboBox _bookGenre;
        [UI]
        private readonly ComboBox _bookPublic;
        [UI]
        private readonly ComboBox _bookAisle;
        [UI]
        private readonly Image _bookImage;

        [UI]
        private readonly SearchEntry _titleSearch;
        [UI]
        private readonly SearchEntry _numberSearch;

        [UI]
        private readonly Button _btnCreateBook;
        [UI]
        private readonly Button _btnUpdateBook;
        [UI]
        private readonly Button _btnDeleteBook;

        private bool _readOnly;

        public TabBooks(Program program, bool readOnly) : this(program, readOnly, new Builder("TabBooks.glade")) { }

        private TabBooks(Program program, bool readOnly, Builder builder) : base(builder.GetRawOwnedObject("TabBooks"))
        {
            this._program = program;
            this._readOnly = readOnly;
            builder.Autoconnect(this);

            Utils.FillEnum(Access.GetInstance().Aisles(), this._aisleCombo, this._bookAisle);
            Utils.FillEnum(Access.GetInstance().Publics(), this._publicCombo, this._bookPublic);
            Utils.FillEnum(Access.GetInstance().Genres(), this._genreCombo, this._bookGenre);
            this.FillBookColumns();
            this.FillBooks();

            this._bookList.CursorChanged += (_, _) =>
            {
                string id = this.GetSelectedBook();

                if (id != null)
                {
                    this.SelectBook(id);
                }
            };

            new List<SearchEntry> { this._titleSearch, this._numberSearch }
                .ForEach(e => e.SearchChanged += (_, _) => { this.FillBooks(); });

            new List<ComboBox> { this._genreCombo, this._aisleCombo, this._publicCombo }
                .ForEach(e => e.Changed += (_, _) => { this.FillBooks(); });

            this._btnCreateBook.Clicked += (_, _) => this.CreateBook();
            this._btnUpdateBook.Clicked += (_, _) => this.UpdateBook();
            this._btnDeleteBook.Clicked += (_, _) => this.DeleteBook();

            if (readOnly)
            {
                this._btnCreateBook.Sensitive = false;
                this._btnUpdateBook.Sensitive = false;
                this._btnDeleteBook.Sensitive = false;

                new List<Entry> { this._bookTitle, this._bookAuthor, this._bookCollection, this._bookIsbn }
                    .ForEach(e => e.Sensitive = false);
                new List<ComboBox> { this._bookGenre, this._bookPublic, this._bookAisle }
                    .ForEach(e => e.Sensitive = false);
            }
            else
            {
                new List<Entry> { this._bookTitle, this._bookAuthor, this._bookCollection, this._bookIsbn }
                    .ForEach(e => e.Changed += (_, _) => { this._btnUpdateBook.Sensitive = true; });

                new List<ComboBox> { this._bookGenre, this._bookPublic, this._bookAisle }
                    .ForEach(e => e.Changed += (_, _) => { this._btnUpdateBook.Sensitive = true; });
            }
        }

        private void FillBookColumns()
        {
            int i = 0;
            this._bookList.AppendColumn(new TreeViewColumn("ID", new CellRendererText() { Weight = 100 }, "text", i++) { Reorderable = true });
            this._bookList.AppendColumn(new TreeViewColumn("Titre", new CellRendererText(), "text", i++));
            this._bookList.AppendColumn(new TreeViewColumn("Auteur", new CellRendererText(), "text", i++));
            this._bookList.AppendColumn(new TreeViewColumn("Collection", new CellRendererText(), "text", i++));
            this._bookList.AppendColumn(new TreeViewColumn("Genre", new CellRendererText(), "text", i++));
            this._bookList.AppendColumn(new TreeViewColumn("Public", new CellRendererText(), "text", i++));
            this._bookList.AppendColumn(new TreeViewColumn("Rayon", new CellRendererText(), "text", i++));
        }

        private async void FillBooks()
        {
            var filters = new Dictionary<string, object>();

            if (this._titleSearch.Text.Length != 0)
            {
                filters.Add("title", this._titleSearch.Text);
            }

            if (this._numberSearch.Text.Length != 0)
            {
                filters.Add("id", this._numberSearch.Text);
            }

            if (this._genreCombo.ActiveId != null)
            {
                filters.Add("idGenre", this._genreCombo.ActiveId);
            }

            if (this._publicCombo.ActiveId != null)
            {
                filters.Add("idPublic", this._publicCombo.ActiveId);
            }

            if (this._aisleCombo.ActiveId != null)
            {
                filters.Add("idRayon", this._aisleCombo.ActiveId);
            }

            var books = await Access.GetInstance().Books().Get(filters);

            ListStore model = new(GLib.GType.String, GLib.GType.String, GLib.GType.String, GLib.GType.String, GLib.GType.String, GLib.GType.String, GLib.GType.String);
            this._bookList.Model = model;

            foreach (Livre b in books)
            {
                int j = 0;
                TreeIter iter = model.Append();
                model.SetValue(iter, j++, b.Id);
                model.SetValue(iter, j++, b.Titre);
                model.SetValue(iter, j++, b.Auteur);
                model.SetValue(iter, j++, b.Collection);
                model.SetValue(iter, j++, b.Genre);
                model.SetValue(iter, j++, b.Public);
                model.SetValue(iter, j++, b.Rayon);
            }
        }

        private async void SelectBook(string id)
        {
            Livre livre = await Access.GetInstance().Books().Get(id);

            this._bookId.Text = livre.Id;
            this._bookTitle.Text = livre.Titre;
            this._bookAuthor.Text = livre.Auteur;
            this._bookCollection.Text = livre.Collection;
            this._bookGenre.ActiveId = livre.IdGenre;
            this._bookPublic.ActiveId = livre.IdPublic;
            this._bookAisle.ActiveId = livre.IdRayon;
            this._bookImagePath.Text = Access.GetImageUrl(livre.Image);
            this._bookIsbn.Text = livre.Isbn;

            this._bookImage.Clear();
            var image = await Access.GetInstance().FetchImage(livre.Image);

            if (image != null)
            {
                this._bookImage.Pixbuf = image.ScaleSimple(256, 256, Gdk.InterpType.Hyper);
            }
            else
            {
                this._bookImage.IconName = "image-missing";
            }
        }

        private void CreateBook()
        {
            CreateBookDialog diag = new(this._program);
            diag.Destroyed += (_, _) => this.FillBooks();
            diag.Run();
        }

        private async void UpdateBook()
        {
            Dictionary<string, object> parameters = new()
            {
                { "titre", this._bookTitle.Text },
                { "auteur", this._bookAuthor.Text },
                { "ISBN", this._bookIsbn.Text },
                { "collection", this._bookCollection.Text },
                { "idRayon", this._bookAisle.ActiveId },
                { "idPublic", this._bookPublic.ActiveId },
                { "idGenre", this._bookGenre.ActiveId },
            };

            await Access.GetInstance().Books().Update(this._bookId.Text, parameters);

            this._btnUpdateBook.Sensitive = false;
            this.FillBooks();
        }

        private void DeleteBook()
        {
            MessageDialog diag = new(null,
                DialogFlags.Modal,
                MessageType.Warning,
                ButtonsType.YesNo,
                true,
                "Êtes vous sûr de vouloir supprimer le livre <b>{0}</b> ?",
                [this._bookTitle.Text]);

            diag.Response += async (object o, ResponseArgs args) =>
            {
                diag.Destroy();

                if (args.ResponseId == ResponseType.Yes)
                {
                    await Access.GetInstance().Books().Delete(this._bookId.Text);
                    this.FillBooks();
                }
            };

            diag.Run();
        }

        private string GetSelectedBook()
        {
            this._bookList.Selection.GetSelected(out TreeIter iter);
            string id = (string)this._bookList.Model.GetValue(iter, 0);
            return id;
        }
    }
}
