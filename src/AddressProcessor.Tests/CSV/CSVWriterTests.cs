using System;
using System.IO;
using NUnit.Framework;
using AddressProcessing.CSV;
using Moq;

namespace AddressProcessing.Tests
{
	[TestFixture]
	public class CSVWriterTests
	{
		[Test]
		public void CSVWriter_WriteValues_Success()
		{
			// Arrange
			var writer = new Mock<TextWriter>();

			string str = null;
			writer.Setup(x => x.WriteLine(It.IsAny<string>())).Callback<string>(msg => str = msg);

			// Act
			using (var csvWriter = new CSVWriter(writer.Object))
			{
				csvWriter.Write("val1", "val2", "val3");
			}

			// Assert
			var sep = CSVConstants.Separator;
			Assert.AreEqual($"val1{sep}val2{sep}val3", str);
		}
	}
}
