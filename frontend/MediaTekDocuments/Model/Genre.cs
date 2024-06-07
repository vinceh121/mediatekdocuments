
namespace MediaTekDocuments.Model
{
    /// <summary>
    /// Classe métier Genre : hérite de Categorie
    /// </summary>
    public class Genre(string id, string libelle) : Categorie(id, libelle)
    {
	}
}
