using System;

namespace MediaTekDocuments.Model
{
    /// <summary>
    /// Classe métier Exemplaire (exemplaire d'une revue)
    /// </summary>
    public class Exemplaire(int numero, DateTime dateAchat, string photo, string idEtat, string idDocument)
	{
		public int Numero { get; set; } = numero;
		public string Photo { get; set; } = photo;
		public DateTime DateAchat { get; set; } = dateAchat;
		public string IdEtat { get; set; } = idEtat;
		public string Id { get; set; } = idDocument;
	}
}
