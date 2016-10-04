using System;
using System.Threading.Tasks;
using AddressProcessing.CSV.Exceptions;

namespace AddressProcessing.CSV
{
    public interface ICsvWriter : IDisposable
    {
        /// <summary>Close the stream writer</summary>
        void Close();

        /// <summary>Write a line to the file async</summary>
        /// <param name="line">The line to write</param>
        void WriteLine(string line);

        /// <summary>Write columns to the CSV</summary>
        /// <param name="columns"></param>
        void Write(string[] columns);

        /// <summary>Open a file for writing</summary>
        /// <param name="filename">The name of the file to open</param>
        /// <exception cref="UnableToOpenFileException">Thrown if there is an exception during file open</exception>
        void Open(string filename);
    }
}