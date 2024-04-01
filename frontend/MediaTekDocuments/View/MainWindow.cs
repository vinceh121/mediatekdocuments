using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
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

		[UI]
		private readonly Entry _bookId;
		[UI]
		private readonly Entry _bookTitle;
		[UI]
		private readonly Entry _bookAuthor;
		[UI]
		private readonly Entry _bookCollection;
		[UI]
		private readonly Entry _bookGenre;
		[UI]
		private readonly Entry _bookPublic;
		[UI]
		private readonly Entry _bookAisle;
		[UI]
		private readonly Entry _bookImagePath;
		[UI]
		private readonly Entry _bookIsbn;
		[UI]
		private readonly Image _bookImage;

		[UI]
		private readonly SearchEntry _titleSearch;
		[UI]
		private readonly SearchEntry _numberSearch;

		[UI]
		private readonly Button _btnUpdateBook;

		public MainWindow(Program program) : this(program, new Builder("MainWindow.glade")) { }

		private MainWindow(Program program, Builder builder) : base(builder.GetRawOwnedObject("MainWindow"))
		{
			this._program = program;
			this.Application = program.GetApplication();
			builder.Autoconnect(this);

			DeleteEvent += Window_DeleteEvent;

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

			this._btnUpdateBook.Clicked += (_, _) => this.UpdateBook();

			new List<Entry> { this._bookTitle, this._bookAuthor, this._bookCollection, this._bookIsbn }
				.ForEach(e => e.Changed += (_, _) => { this._btnUpdateBook.Sensitive = true; });

			this.FillAisles();
			this.FillPublics();
			this.FillGenres();
			this.FillBookColumns();
			this.FillBooks();
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
				filters.Add("genre", this._genreCombo.ActiveId);
			}

			if (this._publicCombo.ActiveId != null)
			{
				filters.Add("public", this._publicCombo.ActiveId);
			}

			if (this._aisleCombo.ActiveId != null)
			{
				filters.Add("aisle", this._aisleCombo.ActiveId);
			}

			var books = await Access.GetInstance().GetLivres(filters);

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
			Livre livre = await Access.GetInstance().GetBook(id);

			this._bookId.Text = livre.Id;
			this._bookTitle.Text = livre.Titre;
			this._bookAuthor.Text = livre.Auteur;
			this._bookCollection.Text = livre.Collection;
			this._bookGenre.Text = livre.Genre;
			this._bookPublic.Text = livre.Public;
			this._bookAisle.Text = livre.Rayon;
			this._bookImagePath.Text = Access.GetImageUrl(livre.Image);
			this._bookIsbn.Text = livre.Isbn;

			this._bookImage.Clear();
			var image = await Access.GetInstance().GetBookImage(livre);

			if (image != null)
			{
				this._bookImage.Pixbuf = image.ScaleSimple(256, 256, Gdk.InterpType.Hyper);
			}
			else
			{
				this._bookImage.IconName = "image-missing";
			}
		}

		private async void UpdateBook()
		{
			Dictionary<string, object> parameters = new()
			{
				{ "titre", this._bookTitle.Text },
				{ "auteur", this._bookAuthor.Text },
				{ "ISBN", this._bookIsbn.Text },
				{ "collection", this._bookCollection.Text }
			};

			await Access.GetInstance().UpdateBook(this._bookId.Text, parameters);

			this._btnUpdateBook.Sensitive = false;
			this.FillBooks();
		}

		private string GetSelectedBook()
		{
			this._bookList.Selection.GetSelected(out TreeIter iter);
			string id = (string)this._bookList.Model.GetValue(iter, 0);
			return id;
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

			cbx.Active = 0;

			cbx.Sensitive = true;
		}
	}
}
