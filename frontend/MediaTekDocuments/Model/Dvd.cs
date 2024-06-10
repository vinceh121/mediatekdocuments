
using Newtonsoft.Json;

namespace MediaTekDocuments.Model
{
    /// <summary>
    /// Classe métier Dvd hérite de LivreDvd : contient des propriétés spécifiques aux dvd
    /// </summary>
    public class Dvd(string id, string titre, string image, int duree, string realisateur, string synopsis,
		string idGenre, string genre, string idPublic, string lePublic, string idRayon, string rayon) : LivreDvd(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon)
    {
		[JsonProperty("duree")]
		public int Duree { get; } = duree;
		[JsonProperty("realisateur")]
		public string Realisateur { get; } = realisateur;
		[JsonProperty("synopsis")]
		public string Synopsis { get; } = synopsis;
	}
}
