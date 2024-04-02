
using Newtonsoft.Json;

namespace MediaTekDocuments.Model
{
    /// <summary>
    /// Classe métier Document (réunit les infomations communes à tous les documents : Livre, Revue, Dvd)
    /// </summary>
    public class Document
    {
        public string Id { get; }
        [JsonProperty("titre")]
        public string Titre { get; }
        [JsonProperty("image")]
        public string Image { get; }
        [JsonProperty("idGenre")]
        public string IdGenre { get; }
        public string Genre { get; }
        [JsonProperty("idPublic")]
        public string IdPublic { get; }
        [JsonProperty("public")] 
        public string Public { get; }
        [JsonProperty("idRayon")]
        public string IdRayon { get; }
        public string Rayon { get; }

        public Document(string id, string titre, string image, string idGenre, string genre, string idPublic, string lePublic, string idRayon, string rayon)
        {
            Id = id;
            Titre = titre;
            Image = image;
            IdGenre = idGenre;
            Genre = genre;
            IdPublic = idPublic;
            Public = lePublic;
            IdRayon = idRayon;
            Rayon = rayon;
        }
    }
}
