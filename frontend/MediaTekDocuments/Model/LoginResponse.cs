using Newtonsoft.Json;

namespace MediaTekDocuments.Model
{
	public class LoginResponse(bool readOnly)
	{
		public bool ReadOnly { get; } = readOnly;
	}
}