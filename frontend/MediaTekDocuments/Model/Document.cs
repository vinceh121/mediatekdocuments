
using Newtonsoft.Json;

namespace MediaTekDocuments.Model
{
    /// <summary>
    /// Classe métier Document (réunit les infomations communes à tous les documents : Livre, Revue, Dvd)
    /// </summary>
    public class Document(string id, string titre, string image, string idGenre, string genre, string idPublic, string lePublic, string idRayon, string rayon)
	{
		public string Id { get; } = id;
		[JsonProperty("titre")]
		public string Titre { get; } = titre;
		[JsonProperty("image")]
		public string Image { get; } = image;
		[JsonProperty("idGenre")]
		public string IdGenre { get; } = idGenre;
		public string Genre { get; } = genre;
		[JsonProperty("idPublic")]
		public string IdPublic { get; } = idPublic;
		[JsonProperty("public")]
		public string Public { get; } = lePublic;
		[JsonProperty("idRayon")]
		public string IdRayon { get; } = idRayon;
		public string Rayon { get; } = rayon;
	}
}
