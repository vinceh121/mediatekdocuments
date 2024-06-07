
using System;

namespace MediaTekDocuments.Model
{
    /// <summary>
    /// Classe métier Categorie (réunit les informations des classes Public, Genre et Rayon)
    /// </summary>
    public class Categorie(string id, string libelle)
	{
		public string Id { get; } = id;
		public string Libelle { get; } = libelle;

		public override bool Equals(object obj)
        {
            return obj is Categorie categorie &&
                   Id == categorie.Id &&
                   Libelle == categorie.Libelle;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Libelle);
        }

        /// <summary>
        /// Récupération du libellé pour l'affichage dans les combos
        /// </summary>
        /// <returns>Libelle</returns>
        public override string ToString()
        {
            return Libelle;
        }
    }
}
