using System;
using System.IO;
using NUnit.Framework;
using AddressProcessing.CSV;
using Moq;

namespace AddressProcessing.Tests
{
	[TestFixture]
	public class CSVReaderTests
	{
		[Test]
		public void CSVReader_ReadTwoColumns_Success()
		{
			// Arrange
			var stream = new Mock<TextReader>();
			stream.Setup(x => x.ReadLine()).Returns($"val1{CSVConstants.Separator}val2");

			string column1 = null, column2 = null;
			bool success;

			// Act
			using (var csvReader = new CSVReader(stream.Object))
			{
				success = csvReader.Read(out column1, out column2);
			}

			// Assert
			Assert.AreEqual(true, success);
			Assert.AreEqual("val1", column1);
			Assert.AreEqual("val2", column2);
		}

		[Test]
		public void CSVReader_ReadOneColumn_Failure()
		{
			// Arrange
			var stream = new Mock<TextReader>();
			stream.Setup(x => x.ReadLine()).Returns("val1");

			string column1 = null, column2 = null;
			bool success;

			// Act
			using (var csvReader = new CSVReader(stream.Object))
			{
				success = csvReader.Read(out column1, out column2);
			}

			// Assert
			Assert.AreEqual(false, success);
			Assert.AreEqual("val1", column1);
			Assert.AreEqual(null, column2);
		}

		[Test]
		public void CSVReader_ReadNoColumns_Failure()
		{
			// Arrange
			var stream = new Mock<TextReader>();
			stream.Setup(x => x.ReadLine()).Returns("");

			string column1 = null, column2 = null;
			bool success;

			// Act
			using (var csvReader = new CSVReader(stream.Object))
			{
				success = csvReader.Read(out column1, out column2);
			}

			// Assert
			Assert.AreEqual(false, success);
			Assert.AreEqual(null, column1);
			Assert.AreEqual(null, column2);
		}
	}
}
