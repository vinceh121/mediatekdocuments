using Gtk;
using MediaTekDocuments.Dal;
using MediaTekDocuments.Model;

namespace MediaTekDocuments.View
{
	public static class Utils
	{
		public static async void FillEnum<T>(ICrud<T> crud, params ComboBox[] cbxs) where T : Categorie
		{
			var datum = await crud.Get();
            ListStore model = new(GLib.GType.String, GLib.GType.String);
            model.AppendValues(null, null);

            foreach (Categorie r in datum)
            {
                model.AppendValues(r.Id, r.Libelle);
            }

			foreach (ComboBox cbx in cbxs)
			{
            	cbx.Model = model;
            	SetComboboxTextRenderer(cbx);
			}
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