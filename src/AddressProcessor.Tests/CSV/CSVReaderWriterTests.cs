using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using AddressProcessing.CSV;

namespace Csv.Tests
{
	[TestFixture]
	public class CSVReaderWriterTests
	{
		private const string testFile = "test1.csv";

		[Test]
		public void CSVReaderWriter_OpenRead_Success()
		{
			// Arrange - create file using writer
			using (var csvReaderWriter = new CSVReaderWriter())
			{
				csvReaderWriter.Open(testFile, CSVReaderWriter.Mode.Write);
			}

			// Act Assert
			using (var csvReaderWriter = new CSVReaderWriter())
			{
				csvReaderWriter.Open(testFile, CSVReaderWriter.Mode.Read);
			}
		}

		[Test]
		public void CSVReaderWriter_OpenWrite_Success()
		{
			// Act Assert
			using (var csvReaderWriter = new CSVReaderWriter())
			{
				csvReaderWriter.Open(testFile, CSVReaderWriter.Mode.Write);
			}
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void CSVReaderWriter_OpenInvalidMode_ThrowsArgumentOutOfRangeException()
		{
			// Act Assert
			using (var csvReaderWriter = new CSVReaderWriter())
			{
				csvReaderWriter.Open(testFile, (CSVReaderWriter.Mode)1234);
			}
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void CSVReaderWriter_ReadBeforeOpenRead_ThrowsInvalidOperationExceptionn()
		{
			// Act Assert
			using (var csvReaderWriter = new CSVReaderWriter())
			{
				string a = null, b = null;
				csvReaderWriter.Read(out a, out b);
			}
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void CSVReaderWriter_WriteBeforeOpenWrite_ThrowsInvalidOperationException()
		{
			// Act Assert
			using (var csvReaderWriter = new CSVReaderWriter())
			{
				csvReaderWriter.Write("a", "b");
			}
		}


		[Test]
		public void CSVReaderWriter_WriteThenRead_Success()
		{
			const string VAL1 = "val1";
			const string VAL2 = "val2";

			using (var csvReaderWriter = new CSVReaderWriter())
			{
				csvReaderWriter.Open(testFile, CSVReaderWriter.Mode.Write);
				csvReaderWriter.Write(VAL1, VAL2);
			}

			using (var csvReaderWriter = new CSVReaderWriter())
			{

				string column1 = null, column2 = null;

				csvReaderWriter.Open(testFile, CSVReaderWriter.Mode.Read);
				var result = csvReaderWriter.Read(out column1, out column2);
				Assert.AreEqual(true, result);
				Assert.AreEqual(VAL1, column1);
				Assert.AreEqual(VAL2, column2);
			}
		}
	
		[Test]
		public void CSVReaderWriter_WriteThenReadWithOneColumn_ReturnsFailure()
		{
			const string VAL1 = "val1";

			using (var csvReaderWriter = new CSVReaderWriter())
			{
				csvReaderWriter.Open(testFile, CSVReaderWriter.Mode.Write);
				csvReaderWriter.Write(VAL1);
			}

			using (var csvReaderWriter = new CSVReaderWriter())
			{

				string column1 = null, column2 = null;

				csvReaderWriter.Open(testFile, CSVReaderWriter.Mode.Read);
				var result = csvReaderWriter.Read(out column1, out column2);
				Assert.AreEqual(false, result);
				Assert.AreEqual(VAL1, column1);
				Assert.AreEqual(null, column2);
			}
		}

		[Test]
		public void CSVReaderWriter_WriteThenReadWithNoColumns_ReturnsFailure()
		{
			using (var csvReaderWriter = new CSVReaderWriter())
			{
				csvReaderWriter.Open(testFile, CSVReaderWriter.Mode.Write);
				csvReaderWriter.Write();
			}

			using (var csvReaderWriter = new CSVReaderWriter())
			{

				string column1 = null, column2 = null;

				csvReaderWriter.Open(testFile, CSVReaderWriter.Mode.Read);
				var result = csvReaderWriter.Read(out column1, out column2);
				Assert.AreEqual(false, result);
				Assert.AreEqual(null, column1);
				Assert.AreEqual(null, column2);
			}
		}
	}
}
