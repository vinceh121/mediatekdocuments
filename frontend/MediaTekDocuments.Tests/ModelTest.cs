using System.Collections.Generic;
using System.IO;
using MediaTekDocuments.Dal;
using MediaTekDocuments.Model;
using Xunit;

namespace MediaTekDocuments.Tests
{
	public class ModelTest
	{
		[Fact]
		public void TestToString()
		{
			Assert.Equal("libelle", new Public("id", "libelle").ToString());
			Assert.Equal("libelle", new Genre("id", "libelle").ToString());
			Assert.Equal("libelle", new Rayon("id", "libelle").ToString());
		}

		[Fact]
		public void TestAccessImage()
		{
			Assert.Equal("http://mediatekdocuments.local/content/image.png", Access.GetImageUrl("image.png"));
		}

		[Fact]
		public void TestMapping()
		{
			var data = "[\r\n\t{\r\n\t\t\"id\": \"10000\",\r\n\t\t\"libelle\": \"Humour\"\r\n\t},\r\n\t{\r\n\t\t\"id\": \"10001\",\r\n\t\t\"libelle\": \"Bande dessinée\"\r\n\t},\r\n\t{\r\n\t\t\"id\": \"10002\",\r\n\t\t\"libelle\": \"Science Fiction\"\r\n\t}\r\n]";

			List<Genre> expected = new() { new Genre("10000", "Humour"), new Genre("10001", "Bande dessinée"), new Genre("10002", "Science Fiction") };

			var stream = new MemoryStream();
			var writer = new StreamWriter(stream);
			writer.Write(data);
			writer.Flush();
			stream.Position = 0;

			Assert.Equal(expected, Access.GetInstance().ParseCollection<Genre>(stream));
		}
	}
}