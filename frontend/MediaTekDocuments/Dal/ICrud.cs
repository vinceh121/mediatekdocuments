using System.Collections.Generic;
using System.Threading.Tasks;

namespace MediaTekDocuments.Dal
{
	public interface ICrud<T>
	{
		public Task Create(T entity);
		public Task<T> Get(string id);
		public Task<List<T>> Get();
		public Task<List<T>> Get(Dictionary<string, object> filters);
		public Task Update(string id, Dictionary<string, object> parameters);
		public Task Delete(string id);
	}
}
