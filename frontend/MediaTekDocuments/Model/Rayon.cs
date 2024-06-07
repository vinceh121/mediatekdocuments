
namespace MediaTekDocuments.Model
{
    /// <summary>
    /// Classe métier Rayon (rayon de classement du document) hérite de Categorie
    /// </summary>
    public class Rayon(string id, string libelle) : Categorie(id, libelle)
    {
	}
}
