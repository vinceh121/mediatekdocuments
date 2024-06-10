using System.Collections.Generic;
using Gtk;
using MediaTekDocuments.Dal;
using MediaTekDocuments.Model;
using UI = Gtk.Builder.ObjectAttribute;

namespace MediaTekDocuments.View.Tabs
{
	public class TabRevue : Container
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
		private readonly Entry _revueId;
		[UI]
		private readonly Entry _revueTitle;
		[UI]
		private readonly SpinButton _revueMiseADispo;
		[UI]
		private readonly ComboBox _revuePeriodicite;
		[UI]
		private readonly Entry _revueImagePath;
		[UI]
		private readonly ComboBox _revueGenre;
		[UI]
		private readonly ComboBox _revuePublic;
		[UI]
		private readonly ComboBox _revueAisle;
		[UI]
		private readonly Image _revueImage;

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

		public TabRevue(Program program, bool readOnly) : this(program, readOnly, new Builder("TabRevue.glade")) { }

		private TabRevue(Program program, bool readOnly, Builder builder) : base(builder.GetRawOwnedObject("TabRevue"))
		{
			this._program = program;
			this._readOnly = readOnly;
			builder.Autoconnect(this);

			Utils.FillEnum(Access.GetInstance().Aisles(), this._aisleCombo, this._revueAisle);
			Utils.FillEnum(Access.GetInstance().Publics(), this._publicCombo, this._revuePublic);
			Utils.FillEnum(Access.GetInstance().Genres(), this._genreCombo, this._revueGenre);
			this.FillBookColumns();
			this.FillRevues();

            CellRendererText txtRender = new();
			this._revuePeriodicite.PackStart(txtRender, true);
			this._revuePeriodicite.SetAttributes(txtRender, "id", 0);
			this._revuePeriodicite.AddAttribute(txtRender, "text", 1);

			this._bookList.CursorChanged += (_, _) =>
			{
				string id = this.GetSelectedBook();

				if (id != null)
				{
					this.SelectBook(id);
				}
			};

			new List<SearchEntry> { this._titleSearch, this._numberSearch }
				.ForEach(e => e.SearchChanged += (_, _) => { this.FillRevues(); });

			new List<ComboBox> { this._genreCombo, this._aisleCombo, this._publicCombo }
				.ForEach(e => e.Changed += (_, _) => { this.FillRevues(); });

			this._btnCreateBook.Clicked += (_, _) => this.CreateRevue();
			this._btnUpdateBook.Clicked += (_, _) => this.UpdateRevue();
			this._btnDeleteBook.Clicked += (_, _) => this.DeleteRevue();

			if (readOnly)
			{
				this._btnCreateBook.Sensitive = false;
				this._btnUpdateBook.Sensitive = false;
				this._btnDeleteBook.Sensitive = false;

				new List<Widget> { this._revueTitle, this._revueMiseADispo, this._revuePeriodicite }
					.ForEach(e => e.Sensitive = false);
				new List<ComboBox> { this._revueGenre, this._revuePublic, this._revueAisle }
					.ForEach(e => e.Sensitive = false);
			}
			else
			{
				new List<IEditable> { this._revueTitle, this._revueMiseADispo }
					.ForEach(e => e.Changed += (_, _) => { this._btnUpdateBook.Sensitive = true; });

				this._revuePeriodicite.Changed += (_, _) => this._btnUpdateBook.Sensitive = true;

				new List<ComboBox> { this._revueGenre, this._revuePublic, this._revueAisle }
					.ForEach(e => e.Changed += (_, _) => { this._btnUpdateBook.Sensitive = true; });
			}
		}

		private void FillBookColumns()
		{
			int i = 0;
			this._bookList.AppendColumn(new TreeViewColumn("ID", new CellRendererText() { Weight = 100 }, "text", i++) { Reorderable = true });
			this._bookList.AppendColumn(new TreeViewColumn("Titre", new CellRendererText(), "text", i++));
			this._bookList.AppendColumn(new TreeViewColumn("Delais mise à dispo", new CellRendererText(), "text", i++));
			this._bookList.AppendColumn(new TreeViewColumn("Périodicité", new CellRendererText(), "text", i++));
			this._bookList.AppendColumn(new TreeViewColumn("Genre", new CellRendererText(), "text", i++));
			this._bookList.AppendColumn(new TreeViewColumn("Public", new CellRendererText(), "text", i++));
			this._bookList.AppendColumn(new TreeViewColumn("Rayon", new CellRendererText(), "text", i++));
		}

		private async void FillRevues()
		{
			var filters = new Dictionary<string, object>();

			if (this._titleSearch.Text.Length != 0)
			{
				filters.Add("titre", this._titleSearch.Text);
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

			var revues = await Access.GetInstance().Revues().Get(filters);

			ListStore model = new(GLib.GType.String, GLib.GType.String, GLib.GType.String, GLib.GType.String, GLib.GType.String, GLib.GType.String, GLib.GType.String);
			this._bookList.Model = model;

			foreach (Revue b in revues)
			{
				int j = 0;
				TreeIter iter = model.Append();
				model.SetValue(iter, j++, b.Id);
				model.SetValue(iter, j++, b.Titre);
				model.SetValue(iter, j++, b.DelaiMiseADispo.ToString());
				model.SetValue(iter, j++, Utils.PeriodiciteName(b.Periodicite));
				model.SetValue(iter, j++, b.Genre);
				model.SetValue(iter, j++, b.Public);
				model.SetValue(iter, j++, b.Rayon);
			}
		}

		private async void SelectBook(string id)
		{
			Revue revue = await Access.GetInstance().Revues().Get(id);

			this._revueId.Text = revue.Id;
			this._revueTitle.Text = revue.Titre;
			this._revueMiseADispo.Value = revue.DelaiMiseADispo;
			this._revuePeriodicite.ActiveId = revue.Periodicite;
			this._revueGenre.ActiveId = revue.IdGenre;
			this._revuePublic.ActiveId = revue.IdPublic;
			this._revueAisle.ActiveId = revue.IdRayon;
			this._revueImagePath.Text = Access.GetImageUrl(revue.Image);

			this._revueImage.Clear();
			var image = await Access.GetInstance().FetchImage(revue.Image);

			if (image != null)
			{
				this._revueImage.Pixbuf = image.ScaleSimple(256, 256, Gdk.InterpType.Hyper);
			}
			else
			{
				this._revueImage.IconName = "image-missing";
			}
		}

		private void CreateRevue()
		{
			CreateBookDialog diag = new(this._program);
			diag.Destroyed += (_, _) => this.FillRevues();
			diag.Run();
		}

		private async void UpdateRevue()
		{
			Dictionary<string, object> parameters = new()
			{
				{ "titre", this._revueTitle.Text },
				{ "auteur", this._revueMiseADispo.Text },
                { "delaisMiseADispo", this._revueMiseADispo.Value },
				{ "periodicite", this._revuePeriodicite.ActiveId },
				{ "idRayon", this._revueAisle.ActiveId },
				{ "idPublic", this._revuePublic.ActiveId },
				{ "idGenre", this._revueGenre.ActiveId },
			};

			await Access.GetInstance().Revues().Update(this._revueId.Text, parameters);

			this._btnUpdateBook.Sensitive = false;
			this.FillRevues();
		}

		private void DeleteRevue()
		{
			MessageDialog diag = new(null,
				DialogFlags.Modal,
				MessageType.Warning,
				ButtonsType.YesNo,
				true,
				"Êtes vous sûr de vouloir supprimer la revue <b>{0}</b> ?",
				[this._revueTitle.Text]);

			diag.Response += async (object o, ResponseArgs args) =>
			{
				diag.Destroy();

				if (args.ResponseId == ResponseType.Yes)
				{
					await Access.GetInstance().Revues().Delete(this._revueId.Text);
					this.FillRevues();
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
