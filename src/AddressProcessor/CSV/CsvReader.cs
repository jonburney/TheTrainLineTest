using System;
using System.IO;
using System.Threading.Tasks;
using AddressProcessing.CSV.Exceptions;
using AddressProcessing.CSV.Models;

namespace AddressProcessing.CSV
{
    public class CsvReader : ICsvReader
    {
        private bool _disposed;
        private StreamReader _csvReader;

        /// <summary>Constructor</summary>
        public CsvReader()
        {
            
        }

        /// <summary>Constructor</summary>
        /// <param name="filename">The name of the file to open</param>
        public CsvReader(string filename)
        {
            Open(filename);
        }

        /// <summary>Validate that we have a valid stream reader before trying to use it</summary>
        private void CheckStreamReaderInit()
        {
            if (_csvReader == null)
            {
                throw new StreamReaderNotInitialisedException("The stream reader has not been initialised. Please call Open() or pass the filename to the constructor");
            }
        }

        /// <summary>Open a file for reading</summary>
        /// <param name="filename">The name of the file to open</param>
        /// <exception cref="UnableToOpenFileException">Thrown if there is an exception during file open</exception>
        public void Open(string filename)
        {
            try
            {
                _csvReader = new StreamReader(filename);
            }
            catch (Exception ex)
            {
                throw new UnableToOpenFileException(ex.Message, ex);
            }
        }

        /// <summary>Read a line from the CSV file</summary>
        /// <returns>Task of String (the line that was read)</returns>
        public async Task<string> ReadLine()
        {
            CheckStreamReaderInit();
            return await _csvReader.ReadLineAsync();
        }

        /// <summary>Close the stream reader</summary>
        public void Close()
        {
            if (_csvReader == null) return;
            _csvReader.Close();
        }

        /// <summary>Dispose any resources</summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>Protected virtual method for disposing, as per best practce to prevent multiple dispose calls</summary>
        /// <param name="disposing">True if we shuld dispose</param>
        protected virtual void Dispose(bool disposing)
        {
            // We don't need to do anything if someone has already called Dispose()
            if (_disposed) return;

            if (disposing)
            {
                if (_csvReader != null) _csvReader.Dispose();
            }

            _disposed = true;
        }

        /// <summary>Read a contact from the CSV file</summary>
        /// <returns>An instance of a Contact() object</returns>
        public async Task<Contact> ReadContactFromCsv()
        {
            string line;
            string[] columns;

            char[] separator = { '\t' };

            line = await ReadLine();

            if (line == null) return null;
            columns = line.Split(separator);

            if (columns.Length == 0) return null;
            
            return new Contact()
            {
                Name = columns[0],
                Address = columns[1]
            };                            
        }
    }
}
