using System;
using System.Collections.Generic;
using MediaTekDocuments.Model;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Specialized;
using System.Web;
using System.Net.Http.Headers;
using System.Collections.Concurrent;
using System.Text.Json.Serialization;

namespace MediaTekDocuments.Dal
{
    /// <summary>
    /// Classe d'accès aux données
    /// </summary>
    public class Access
    {
        public static readonly MediaTypeHeaderValue jsonMimeType = new("application/json");

#if DEBUG
        public static readonly string baseUri = "http://localhost:8080";
#else
        public static readonly string baseUri = "https://mediatekdocuments.vinceh121.me";
#endif

        public static readonly string imageUri = baseUri + "/content/";

        /// <summary>
        /// adresse de l'API
        /// </summary>
        public static readonly string uriApi = baseUri + "/api/v1/";
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
            this._client = new HttpClient() { BaseAddress = new Uri(uriApi) };
            this._client.DefaultRequestHeaders.Add("Accept", "application/json");

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

        public async Task<LoginResponse> Login(string email, string password)
        {
            var body = JsonConvert.SerializeObject(new Dictionary<string, string> { { "email", email }, { "password", password } });
            var res = await this._client.PostAsync("security/login", new StringContent(body, jsonMimeType));

            if (res.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                var err = this.ParseObject<ApiError>(await res.Content.ReadAsStreamAsync());
                throw new Exception(err.Messages.Error);
            }
            else if (res.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var loginRes = this.ParseObject<LoginResponse>(await res.Content.ReadAsStreamAsync());

                return loginRes;
            }
            else
            {
                res.EnsureSuccessStatusCode();
                return null; // never returns, always throws
            }
        }

        public ICrud<Livre> Books()
        {
            return new SimpleCrud<Livre>("books", this);
        }

        public ICrud<Dvd> Dvds()
        {
            return new SimpleCrud<Dvd>("dvds", this);
        }

        public ICrud<Revue> Revues()
        {
            return new SimpleCrud<Revue>("revues", this);
        }

        public ICrud<Public> Publics()
        {
            return new SimpleCrud<Public>("publics", this);
        }

        public ICrud<Rayon> Aisles()
        {
            return new SimpleCrud<Rayon>("aisles", this);
        }

        public ICrud<Genre> Genres()
        {
            return new SimpleCrud<Genre>("genres", this);
        }

        public async Task<Gdk.Pixbuf> FetchImage(string imagePath)
        {
            try
            {
                var stream = await this._client.GetStreamAsync(GetImageUrl(imagePath));
                return new Gdk.Pixbuf(stream);
            }
            catch (HttpRequestException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                throw;
            }
        }

        public List<T> ParseCollection<T>(Stream stream)
        {
            return this._serializer.Deserialize<List<T>>(new JsonTextReader(new StreamReader(stream)));
        }

        public T ParseObject<T>(Stream stream)
        {
            return this._serializer.Deserialize<T>(new JsonTextReader(new StreamReader(stream)));
        }

        public JsonSerializer GetSerializer()
        {
            return this._serializer;
        }

        public HttpClient GetClient()
        {
            return this._client;
        }

        public static string GetImageUrl(string imagePath)
        {
            return imageUri + imagePath;
        }
    }
}
