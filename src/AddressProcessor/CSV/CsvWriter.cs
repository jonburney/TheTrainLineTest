using System;
using System.IO;
using System.Threading.Tasks;
using AddressProcessing.CSV.Exceptions;

namespace AddressProcessing.CSV
{
    public class CsvWriter : ICsvWriter
    {

        private bool _disposed;
        private StreamWriter _csvWriter;

        /// <summary>Constructor</summary>
        public CsvWriter()
        {
            
        }

        /// <summary>Constructor</summary>
        /// <param name="filename">The name of the file to open</param>
        public CsvWriter(string filename)
        {
            Open(filename);
        }

        /// <summary>Validate that we have a valid stream writer before trying to use it</summary>
        private void CheckStreamWriterInit()
        {
            if (_csvWriter == null)
            {
                throw new StreamWriterNotInitialisedException("The stream writer has not been initialised. Please call Open() or pass the filename to the constructor");
            }
        }

        /// <summary>Write columns to the CSV</summary>
        /// <param name="columns"></param>
        public void Write(string[] columns)
        {
            string outPut = "";

            for (int i = 0; i < columns.Length; i++)
            {
                outPut += columns[i];
                if ((columns.Length - 1) != i)
                {
                    outPut += "\t";
                }
            }
            
            // Ideally we would use await here but we need to make this method async which will break b/c
            WriteLine(outPut).RunSynchronously();
        }

        /// <summary>Open a file for writing</summary>
        /// <param name="filename">The name of the file to open</param>
        /// <exception cref="UnableToOpenFileException">Thrown if there is an exception during file open</exception>
        public void Open(string filename)
        {
            try
            {
                _csvWriter = new StreamWriter(filename);
            }
            catch (Exception ex)
            {
                throw new UnableToOpenFileException(ex.Message, ex);
            }
        }

        /// <summary>Close the stream writer</summary>
        public void Close()
        {
            if (_csvWriter == null) return;
            _csvWriter.Close();
        }

        /// <summary>Write a line to the file async</summary>
        /// <param name="line">The line to write</param>
        public async Task WriteLine(string line)
        {
            CheckStreamWriterInit();
            await _csvWriter.WriteLineAsync(line);
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
                if (_csvWriter != null) _csvWriter.Dispose();
            }

            _disposed = true;
        }
    }

   

}
