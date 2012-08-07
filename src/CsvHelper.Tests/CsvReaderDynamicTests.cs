using System.Collections.Generic;
using CsvHelper.Configuration;
using Moq;
using Xunit;

namespace CsvHelper.Tests
{
	public class CsvReaderDynamicTests
	{
		[Fact]
		public void GetRecordDynamicTest()
		{
			var data = new List<string[]>
			{
				new[] { "Id", "Name" },
				new[] { "1", "one" },
				new[] { "2", "two" },
				null
			};

			var parserMock = new Mock<ICsvParser>();
			parserMock.Setup( m => m.Configuration ).Returns( new CsvConfiguration() );
			var i = -1;
			parserMock.Setup( m => m.Read() ).Returns( () =>
			{
				i++;
				return data[i];
			} );

			var reader = new CsvReader( parserMock.Object ) as ICsvReader;
			reader.Read();
// ReSharper disable SuggestUseVarKeywordEverywhere
			// NCrunch can't build a dyanmic from another assembly using var.
			dynamic record = reader.GetRecordDynamic();
// ReSharper restore SuggestUseVarKeywordEverywhere

			Assert.NotNull( record );
			Assert.Equal( 1, (int)record.Id );
			Assert.Equal( "one", (string)record.Name );

			reader.Read();
			record = reader.GetRecordDynamic();

			Assert.NotNull( record );
			Assert.Equal( 2, record.Id );
			Assert.Equal( "two", record.Name );

			Assert.False( reader.Read() );
		}
	}
}
