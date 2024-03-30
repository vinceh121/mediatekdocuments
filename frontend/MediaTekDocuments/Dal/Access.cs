using System;
using System.Collections.Generic;
using MediaTekDocuments.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;

namespace MediaTekDocuments.dal
{
    /// <summary>
    /// Classe d'accès aux données
    /// </summary>
    public class Access
    {
        /// <summary>
        /// adresse de l'API
        /// </summary>
        private static readonly string uriApi = "http://mediatekdocuments.local/api/v1/";
        /// <summary>
        /// instance unique de la classe
        /// </summary>
        private static readonly Access _instance = new();

        private readonly JsonSerializer _serializer;
        private readonly HttpClient _client;

        /// <summary>
        /// Méthode privée pour créer un singleton
        /// initialise l'accès à l'API
        /// </summary>
        private Access()
        {
            const String authenticationString = "admin:adminpwd";

            _client = new HttpClient() { BaseAddress = new Uri(uriApi) };

            if (!String.IsNullOrEmpty(authenticationString))
            {
                String base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));
                _client.DefaultRequestHeaders.Add("Authorization", "Basic " + base64EncodedAuthenticationString);
            }

            this._serializer = JsonSerializer.CreateDefault();
        }

        /// <summary>
        /// Création et retour de l'instance unique de la classe
        /// </summary>
        /// <returns>instance unique de la classe</returns>
        public static Access GetInstance()
        {
            return _instance;
        }

        /// <summary>
        /// Retourne tous les genres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public async Task<List<Genre>> GetAllGenres()
        {
            var stream = await this._client.GetStreamAsync("genres");
            return this.ParseCollection<Genre>(stream);
        }

        /// <summary>
        /// Retourne tous les rayons à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public async Task<List<Rayon>> GetAllRayons()
        {
            var stream = await this._client.GetStreamAsync("aisles");
            return this.ParseCollection<Rayon>(stream);
        }

        /// <summary>
        /// Retourne toutes les catégories de public à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public async Task<List<Public>> GetAllPublics()
        {
            var stream = await this._client.GetStreamAsync("publics");
            return this.ParseCollection<Public>(stream);
        }

        /// <summary>
        /// Retourne toutes les livres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public async Task<List<Livre>> GetAllLivres()
        {
            var stream = await this._client.GetStreamAsync("books");
            return this.ParseCollection<Livre>(stream);
        }

        /// <summary>
        /// Retourne toutes les dvd à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Dvd</returns>
        public async Task<List<Dvd>> GetAllDvd()
        {
            var stream = await this._client.GetStreamAsync("dvd");
            return this.ParseCollection<Dvd>(stream);
        }

        /// <summary>
        /// Retourne toutes les revues à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public async Task<List<Revue>> GetAllRevues()
        {
            var stream = await this._client.GetStreamAsync("revues");
            return this.ParseCollection<Revue>(stream);
        }


        /// <summary>
        /// Retourne les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocument">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public async Task<List<Exemplaire>> GetExemplairesRevue(string idDocument)
        {
            var stream = await this._client.GetStreamAsync("exemplaires?id=" + idDocument);
            return this.ParseCollection<Exemplaire>(stream);
        }

        /// <summary>
        /// ecriture d'un exemplaire en base de données
        /// </summary>
        /// <param name="exemplaire">exemplaire à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public async void CreerExemplaire(Exemplaire exemplaire)
        {
            String body = JsonConvert.SerializeObject(exemplaire);

            await this._client.PostAsync("exemplaire", new StringContent(body));
        }

        private List<T> ParseCollection<T>(Stream stream)
        {
            return this._serializer.Deserialize<List<T>>(new JsonTextReader(new StreamReader(stream)));
        }

        private T ParseObject<T>(Stream stream)
        {
            return this._serializer.Deserialize<T>(new JsonTextReader(new StreamReader(stream)));
        }
    }
}
