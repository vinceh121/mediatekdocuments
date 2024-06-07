
namespace MediaTekDocuments.Model
{
	/// <summary>
	/// Classe métier LivreDvd hérite de Document
	/// </summary>
	public abstract class LivreDvd(string id, string titre, string image, string idGenre, string genre,
		string idPublic, string lePublic, string idRayon, string rayon) : Document(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon)
	{
	}
}
