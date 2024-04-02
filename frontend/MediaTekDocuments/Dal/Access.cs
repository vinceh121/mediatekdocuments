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

namespace MediaTekDocuments.dal
{
	/// <summary>
	/// Classe d'accès aux données
	/// </summary>
	public class Access
	{
		private static readonly MediaTypeHeaderValue jsonMimeType = new("application/json");
		private static readonly string imageUri = "http://mediatekdocuments.local/content/";
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

		public async Task<bool> Login(string email, string password)
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
				return true;
			}
			else
			{
				res.EnsureSuccessStatusCode();
				return false; // never returns, always throws
			}
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

		public async Task<List<Livre>> GetLivres(Dictionary<string, object> filters)
		{
			NameValueCollection query = HttpUtility.ParseQueryString(String.Empty);

			foreach (KeyValuePair<string, object> p in filters)
			{
				query.Add(p.Key, p.Value?.ToString());
			}

			var stream = await this._client.GetStreamAsync("books?" + query.ToString());
			return this.ParseCollection<Livre>(stream);
		}

		public async Task<Livre> GetBook(string id)
		{
			var stream = await this._client.GetStreamAsync("books/" + id);
			return this.ParseObject<Livre>(stream);
		}

		public async Task<Gdk.Pixbuf> GetBookImage(Livre l)
		{
			try
			{
				var stream = await this._client.GetStreamAsync(GetImageUrl(l.Image));
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

		public async Task CreateBook(Livre book)
		{
			var body = JsonConvert.SerializeObject(book, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
			var res = await this._client.PostAsync("books", new StringContent(body, jsonMimeType));

			res.EnsureSuccessStatusCode();
		}

		public async Task UpdateBook(string id, Dictionary<string, object> parameters)
		{
			var body = JsonConvert.SerializeObject(parameters);
			await this._client.PatchAsync("books/" + id, new StringContent(body, jsonMimeType));
		}

		public async Task UpdateBook(string id, string field, string value)
		{
			var body = JsonConvert.SerializeObject(new Dictionary<object, object>() { { field, value } });
			await this._client.PatchAsync("books/" + id, new StringContent(body, jsonMimeType));
		}

		public async Task DeleteBook(string id)
		{
			await this._client.DeleteAsync("books/" + id);
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

		public static string GetImageUrl(string imagePath)
		{
			return imageUri + imagePath;
		}
	}
}
