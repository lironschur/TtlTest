using System;
using System.IO;

namespace AddressProcessing.CSV
{
    /*
        2) Refactor this class into clean, elegant, rock-solid & well performing code, without over-engineering.
           Assume this code is in production and backwards compatibility must be maintained.
    */

    public class CSVReaderWriter : IDisposable
    {
        private CSVReader _reader = null;
        private CSVWriter _writer = null;

        [Flags]
        public enum Mode { Read = 1, Write = 2 };

		public static readonly char Separator = '\t'; 

        public void Open(string fileName, Mode mode)
        {
			switch(mode)
			{
				case Mode.Read:
                	_reader = new CSVReader(fileName);
					return;

				case Mode.Write:
					_writer = new CSVWriter(fileName);
					return;

				default:
					// used to throw Exception but this shouldn't break legacy users
                	throw new ArgumentOutOfRangeException("Unknown file mode for " + fileName);
            }
        }

		// we could write more than 2 columns (but we read only 2); kept for backward compatibility
        public void Write(params string[] columns)
        {
			if (_writer == null)
				throw new InvalidOperationException("You must call Open for write first");

			_writer.Write(columns);
        }

		// this method should be parameterless but we're keeping the parameters for backward compatibility
        public bool Read(string column1 = null, string column2 = null)
        {
			return Read(out column1, out column2);
        }

        public bool Read(out string column1, out string column2)
        {
			if (_reader == null)
				throw new InvalidOperationException("You must call Open for read first");

			return _reader.Read(out column1, out column2);
        }

        public void Close()
		{
			_reader?.Dispose();
			_writer?.Dispose();
        }

		private bool isDisposed = false;
		public void Dispose()
		{
			if (!isDisposed)
			{
				Close();
				isDisposed = true;
			}
		}
    }
}
