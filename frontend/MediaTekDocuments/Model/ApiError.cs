using Newtonsoft.Json;

namespace MediaTekDocuments.Model
{
	public class ApiError(Messages messages)
	{
		[JsonProperty("messages")]
		public Messages Messages { get; } = messages;
	}

	public class Messages(string error)
	{
		[JsonProperty("error")]
		public string Error { get; } = error;
	}
}