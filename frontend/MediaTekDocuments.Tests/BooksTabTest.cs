using System;
using Gtk;
using MediaTekDocuments.View;
using Xunit;

namespace MediaTekDocuments.Tests
{
	public class BooksTabTest
	{
		private Program _program;
		private MainWindow _win;

		public BooksTabTest()
		{
			this._program = new();
			this._win = new(this._program, true);
		}

		[Fact]
		public void TestFilledComboBoxes()
		{
			var genre = TestUtils.GetChildByName<ComboBox>(this._win, "_genreCombo");
			Assert.NotNull(genre);
			Assert.NotNull(genre.Model);
			
			var count = ((ListStore)genre.Model).Data.Count;

			Assert.NotEqual(0, count);
		}

		[Fact]
		public void TestFilledBookTree()
		{
			var tree = TestUtils.GetChildByName<TreeView>(this._win, "_bookList");
			Assert.NotNull(tree);
			Assert.NotNull(tree.Model);

			var count = ((ListStore)tree.Model).Data.Count;

			Assert.NotEqual(0, count);
		}
	}
}