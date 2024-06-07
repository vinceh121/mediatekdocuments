namespace MediaTekDocuments.Dal
{
	public class SimpleCrud<T> : AbstractCrud<T>
	{
		public SimpleCrud(string entity, Access access) : base(entity, access)
		{
		}
	}
}
