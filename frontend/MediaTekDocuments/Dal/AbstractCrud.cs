using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace MediaTekDocuments.Dal
{
    public abstract class AbstractCrud<T> : ICrud<T>
    {
        protected readonly string _entity;

        protected readonly Access _access;
        protected readonly JsonSerializer _serializer;
        protected readonly HttpClient _client;

        public AbstractCrud(string entity, Access access)
        {
            this._entity = entity;
            this._access = access;
            this._serializer = access.GetSerializer();
            this._client = access.GetClient();
        }

        public async Task Create(T entity)
        {
            var body = JsonConvert.SerializeObject(entity, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            var res = await this._client.PostAsync(this._entity, new StringContent(body, Access.jsonMimeType));

            res.EnsureSuccessStatusCode();
        }

        public async Task<T> Get(string id)
        {
            var stream = await this._client.GetStreamAsync(this._entity + "/" + id);
            return this._access.ParseObject<T>(stream);
        }

        public async Task<List<T>> Get()
        {
            var stream = await this._client.GetStreamAsync(this._entity);
            return this._access.ParseCollection<T>(stream);
        }

        public async Task<List<T>> Get(Dictionary<string, object> filters)
        {
            NameValueCollection query = HttpUtility.ParseQueryString(String.Empty);

            foreach (KeyValuePair<string, object> p in filters)
            {
                query.Add(p.Key, p.Value?.ToString());
            }

            var stream = await this._client.GetStreamAsync(this._entity + "?" + query.ToString());
            return this._access.ParseCollection<T>(stream);
        }

        public async Task Update(string id, Dictionary<string, object> parameters)
        {
            var body = JsonConvert.SerializeObject(parameters);
            var res = await this._client.PatchAsync(this._entity + "/" + id, new StringContent(body, Access.jsonMimeType));
            res.EnsureSuccessStatusCode();
        }

        public async Task Delete(string id)
        {
            var res = await this._client.DeleteAsync(this._entity + "/" + id);
            res.EnsureSuccessStatusCode();
        }
    }
}
