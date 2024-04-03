using Gtk;

namespace MediaTekDocuments.Tests
{
	public sealed class TestUtils
	{
		public static T GetChildByName<T>(Container container, string name) where T : Widget
		{
			foreach (Widget child in container)
			{
				if (child.Name == name)
				{
					return (T)child;
				}
				else if (child is Container)
				{
					var maybeChild = GetChildByName<T>((Container)child, name);
					if (maybeChild != null)
					{
						return (T)maybeChild;
					}
				}
			}

			return null;
		}
	}
}