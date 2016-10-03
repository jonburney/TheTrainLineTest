using System;
using System.Threading.Tasks;
using AddressProcessing.CSV.Exceptions;
using AddressProcessing.CSV.Models;

namespace AddressProcessing.CSV
{
    public interface ICsvReader : IDisposable
    {
        /// <summary>Read a line from the CSV file</summary>
        /// <returns>Task of String (the line that was read)</returns>
        Task<string> ReadLine();

        /// <summary>Close the stream reader</summary>
        void Close();

        /// <summary>Read a contact from the CSV file</summary>
        /// <returns>An instance of a Contact() object</returns>
        Task<Contact> ReadContactFromCsv();

        /// <summary>Open a file for reading</summary>
        /// <param name="filename">The name of the file to open</param>
        /// <exception cref="UnableToOpenFileException">Thrown if there is an exception during file open</exception>
        void Open(string filename);
    }
}