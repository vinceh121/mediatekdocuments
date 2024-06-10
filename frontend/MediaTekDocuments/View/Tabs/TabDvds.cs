using System;
using System.Collections.Generic;
using Gtk;
using MediaTekDocuments.Dal;
using MediaTekDocuments.Model;
using UI = Gtk.Builder.ObjectAttribute;

namespace MediaTekDocuments.View.Tabs
{
	public class TabDvds : Container
	{
		private readonly Program _program;

		[UI]
		private readonly ComboBox _aisleCombo;
		[UI]
		private readonly ComboBox _publicCombo;
		[UI]
		private readonly ComboBox _genreCombo;
		[UI]
		private readonly TreeView _dvdList;

		[UI]
		private readonly Entry _dvdId;
		[UI]
		private readonly Entry _dvdTitle;
		[UI]
		private readonly Entry _dvdDirector;
		[UI]
		private readonly SpinButton _dvdDuration;
		[UI]
		private readonly Entry _dvdImagePath;
		[UI]
		private readonly TextView _dvdSynopsis;
		[UI]
		private readonly ComboBox _dvdGenre;
		[UI]
		private readonly ComboBox _dvdPublic;
		[UI]
		private readonly ComboBox _dvdAisle;
		[UI]
		private readonly Image _dvdImage;

		[UI]
		private readonly SearchEntry _titleSearch;
		[UI]
		private readonly SearchEntry _numberSearch;

		[UI]
		private readonly Button _btnCreateDvd;
		[UI]
		private readonly Button _btnUpdateDvd;
		[UI]
		private readonly Button _btnDeleteDvd;

		private bool _readOnly;

		public TabDvds(Program program, bool readOnly) : this(program, readOnly, new Builder("TabDvds.glade")) { }

		private TabDvds(Program program, bool readOnly, Builder builder) : base(builder.GetRawOwnedObject("TabDvds"))
		{
			this._program = program;
			this._readOnly = readOnly;
			builder.Autoconnect(this);

			Utils.FillEnum(Access.GetInstance().Aisles(), this._aisleCombo, this._dvdAisle);
			Utils.FillEnum(Access.GetInstance().Publics(), this._publicCombo, this._dvdPublic);
			Utils.FillEnum(Access.GetInstance().Genres(), this._genreCombo, this._dvdGenre);
			this.FillDvdColumns();
			this.FillDvds();

			this._dvdList.CursorChanged += (_, _) =>
			{
				string id = this.GetSelectedDvd();

				if (id != null)
				{
					this.SelectDvd(id);
				}
			};

			new List<SearchEntry> { this._titleSearch, this._numberSearch }
				.ForEach(e => e.SearchChanged += (_, _) => { this.FillDvds(); });

			new List<ComboBox> { this._genreCombo, this._aisleCombo, this._publicCombo }
				.ForEach(e => e.Changed += (_, _) => { this.FillDvds(); });

			this._btnCreateDvd.Clicked += (_, _) => this.CreateDvd();
			this._btnUpdateDvd.Clicked += (_, _) => this.UpdateDvds();
			this._btnDeleteDvd.Clicked += (_, _) => this.DeleteDvd();

			if (readOnly)
			{
				this._btnCreateDvd.Sensitive = false;
				this._btnUpdateDvd.Sensitive = false;
				this._btnDeleteDvd.Sensitive = false;

				new List<Widget> { this._dvdTitle, this._dvdDirector, this._dvdDuration, this._dvdSynopsis }
					.ForEach(e => e.Sensitive = false);
				new List<ComboBox> { this._dvdGenre, this._dvdPublic, this._dvdAisle }
					.ForEach(e => e.Sensitive = false);
			}
			else
			{
				new List<Entry> { this._dvdTitle, this._dvdDirector, this._dvdDuration }
					.ForEach(e => e.Changed += (_, _) => { this._btnUpdateDvd.Sensitive = true; });

				this._dvdSynopsis.Buffer.Changed += (_, _) => this._btnUpdateDvd.Sensitive = true;

				new List<ComboBox> { this._dvdGenre, this._dvdPublic, this._dvdAisle }
					.ForEach(e => e.Changed += (_, _) => { this._btnUpdateDvd.Sensitive = true; });
			}
		}

		private void FillDvdColumns()
		{
			int i = 0;
			this._dvdList.AppendColumn(new TreeViewColumn("ID", new CellRendererText() { Weight = 100 }, "text", i++) { Reorderable = true });
			this._dvdList.AppendColumn(new TreeViewColumn("Titre", new CellRendererText(), "text", i++));
			this._dvdList.AppendColumn(new TreeViewColumn("Realisateur", new CellRendererText(), "text", i++));
			this._dvdList.AppendColumn(new TreeViewColumn("Durée", new CellRendererText(), "text", i++));
			this._dvdList.AppendColumn(new TreeViewColumn("Genre", new CellRendererText(), "text", i++));
			this._dvdList.AppendColumn(new TreeViewColumn("Public", new CellRendererText(), "text", i++));
			this._dvdList.AppendColumn(new TreeViewColumn("Rayon", new CellRendererText(), "text", i++));
		}

		private async void FillDvds()
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

			var dvds = await Access.GetInstance().Dvds().Get(filters);

			ListStore model = new(GLib.GType.String, GLib.GType.String, GLib.GType.String, GLib.GType.String, GLib.GType.String, GLib.GType.String, GLib.GType.String);
			this._dvdList.Model = model;

			foreach (Dvd b in dvds)
			{
				int j = 0;
				TreeIter iter = model.Append();
				model.SetValue(iter, j++, b.Id);
				model.SetValue(iter, j++, b.Titre);
				model.SetValue(iter, j++, b.Realisateur);
				model.SetValue(iter, j++, TimeSpan.FromMinutes(b.Duree).ToString());
				model.SetValue(iter, j++, b.Genre);
				model.SetValue(iter, j++, b.Public);
				model.SetValue(iter, j++, b.Rayon);
			}
		}

		private async void SelectDvd(string id)
		{
			Dvd dvd = await Access.GetInstance().Dvds().Get(id);

			this._dvdId.Text = dvd.Id;
			this._dvdTitle.Text = dvd.Titre;
			this._dvdDirector.Text = dvd.Realisateur;
			this._dvdDuration.Value = dvd.Duree;
			this._dvdGenre.ActiveId = dvd.IdGenre;
			this._dvdPublic.ActiveId = dvd.IdPublic;
			this._dvdAisle.ActiveId = dvd.IdRayon;
			this._dvdImagePath.Text = Access.GetImageUrl(dvd.Image);
			this._dvdSynopsis.Buffer.Text = dvd.Synopsis;

			this._dvdImage.Clear();
			var image = await Access.GetInstance().FetchImage(dvd.Image);

			if (image != null)
			{
				this._dvdImage.Pixbuf = image.ScaleSimple(256, 256, Gdk.InterpType.Hyper);
			}
			else
			{
				this._dvdImage.IconName = "image-missing";
			}
		}

		private void CreateDvd()
		{
			CreateDvdDialog diag = new(this._program);
			diag.Destroyed += (_, _) => this.FillDvds();
			diag.Run();
		}

		private async void UpdateDvds()
		{
			Dictionary<string, object> parameters = new()
			{
				{ "titre", this._dvdTitle.Text },
				{ "realisateur", this._dvdDirector.Text },
				{ "synopsis", this._dvdSynopsis.Buffer.Text },
				{ "duree", this._dvdDuration.Value },
				{ "idRayon", this._dvdAisle.ActiveId },
				{ "idPublic", this._dvdPublic.ActiveId },
				{ "idGenre", this._dvdGenre.ActiveId },
			};

			await Access.GetInstance().Dvds().Update(this._dvdId.Text, parameters);

			this._btnUpdateDvd.Sensitive = false;
			this.FillDvds();
		}

		private void DeleteDvd()
		{
			MessageDialog diag = new(null,
				DialogFlags.Modal,
				MessageType.Warning,
				ButtonsType.YesNo,
				true,
				"Êtes vous sûr de vouloir supprimer le DVD <b>{0}</b> ?",
				[this._dvdTitle.Text]);

			diag.Response += async (object o, ResponseArgs args) =>
			{
				diag.Destroy();

				if (args.ResponseId == ResponseType.Yes)
				{
					await Access.GetInstance().Dvds().Delete(this._dvdId.Text);
					this.FillDvds();
				}
			};

			diag.Run();
		}

		private string GetSelectedDvd()
		{
			this._dvdList.Selection.GetSelected(out TreeIter iter);
			string id = (string)this._dvdList.Model.GetValue(iter, 0);
			return id;
		}
	}
}
