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
        private StreamReader _readerStream = null;
        private StreamWriter _writerStream = null;

        [Flags]
        public enum Mode { Read = 1, Write = 2 };

		public static readonly char Separator = '\t'; 

        public void Open(string fileName, Mode mode)
        {
			switch(mode)
			{
				case Mode.Read:
                	_readerStream = File.OpenText(fileName);
					return;

				case Mode.Write:
					var fileInfo = new FileInfo(fileName);
					_writerStream = fileInfo.CreateText();
					return;

				default:
					// used to throw Exception but this shouldn't break legacy users
                	throw new ArgumentOutOfRangeException("Unknown file mode for " + fileName);
            }
        }

		// we could write more than 2 columns (but we read only 2); kept for backward compatibility
        public void Write(params string[] columns)
        {
			var outPut = String.Join(Separator.ToString(), columns);
            _writerStream.WriteLine(outPut);
        }

		// this method should be parameterless but we're keeping the parameters for backward compatibility
        public bool Read(string column1 = null, string column2 = null)
        {
			return Read(out column1, out column2);
        }

        public bool Read(out string column1, out string column2)
        {
			column1 = null;
			column2 = null;

			var line = _readerStream.ReadLine();
			if (string.IsNullOrEmpty(line))
            {
                return false;
            }

            var columns = line.Split(Separator);
			column1 = columns.Length > 0 ? columns[0] : null;
            column2 = columns.Length > 1 ? columns[1] : null;
            return columns.Length > 1;
        }

        public void Close()
        {
            if (_writerStream != null)
            {
                _writerStream.Close();
				_writerStream = null;
            }

            if (_readerStream != null)
            {
                _readerStream.Close();
				_readerStream = null;
            }
        }

		public void Dispose()
		{
			Close();
		}
    }
}
