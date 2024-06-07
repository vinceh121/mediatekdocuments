
using Newtonsoft.Json;

namespace MediaTekDocuments.Model
{
    /// <summary>
    /// Classe métier Livre hérite de LivreDvd : contient des propriétés spécifiques aux livres
    /// </summary>
    public class Livre(string id, string titre, string image, string isbn, string auteur, string collection,
		string idGenre, string genre, string idPublic, [JsonProperty("public")] string lePublic, string idRayon, string rayon) : LivreDvd(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon)
    {
		[JsonProperty("ISBN")]
		public string Isbn { get; } = isbn;
		[JsonProperty("auteur")]
		public string Auteur { get; } = auteur;
		[JsonProperty("collection")]
		public string Collection { get; } = collection;
	}
}
