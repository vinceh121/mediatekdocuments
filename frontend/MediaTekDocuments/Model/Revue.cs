
namespace MediaTekDocuments.Model
{
    /// <summary>
    /// Classe métier Revue hérite de Document : contient des propriétés spécifiques aux revues
    /// </summary>
    public class Revue(string id, string titre, string image, string idGenre, string genre,
        string idPublic, string lePublic, string idRayon, string rayon,
        string periodicite, int delaiMiseADispo) : Document(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon)
    {
        public string Periodicite { get; set; } = periodicite;
        public int DelaiMiseADispo { get; set; } = delaiMiseADispo;
    }
}
