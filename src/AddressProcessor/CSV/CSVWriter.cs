using System;
using System.IO;

namespace AddressProcessing
{
	public class CSVWriter : IDisposable
	{
		private TextWriter _writer = null;

		public CSVWriter(string fileName)
		{
			var fileInfo = new FileInfo(fileName);
			_writer = fileInfo.CreateText();
		}

		public CSVWriter(TextWriter writer)
		{
			_writer = writer;
		}

		public void Write(params string[] columns)
		{
			var outPut = String.Join(CSVConstants.Separator.ToString(), columns);
			_writer.WriteLine(outPut);
		}

		private bool isDisposed = false;
		public void Dispose()
		{
			if (!isDisposed)
			{
				if (_writer != null)
				{
					_writer.Close();
					_writer = null;
				}
				isDisposed = true;
			}
		}
	}
}
