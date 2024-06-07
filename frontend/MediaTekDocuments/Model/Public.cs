
namespace MediaTekDocuments.Model
{
    /// <summary>
    /// Classe métier Public (public concerné par le document) hérite de Categorie
    /// </summary>
    public class Public(string id, string libelle) : Categorie(id, libelle)
    {
	}
}
