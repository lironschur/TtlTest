using System;
using System.IO;

namespace AddressProcessing
{
	public class CSVReader : IDisposable
	{
		private TextReader _reader = null;

		public CSVReader(string fileName)
		{
			_reader = File.OpenText(fileName);
		}

		public CSVReader(TextReader reader)
		{
			_reader = reader;
		}


		public bool Read(out string column1, out string column2)
		{
			column1 = null;
			column2 = null;

			var line = _reader.ReadLine();
			if (string.IsNullOrEmpty(line))
			{
				return false;
			}

			var columns = line.Split(CSVConstants.Separator);
			column1 = columns.Length > 0 ? columns[0] : null;
			column2 = columns.Length > 1 ? columns[1] : null;
			return columns.Length > 1;
		}

		private bool isDisposed = false;
		public void Dispose()
		{
			if (!isDisposed)
			{
				if (_reader != null)
				{
					_reader.Close();
					_reader = null;
				}
				isDisposed = true;
			}
		}
	}
}
