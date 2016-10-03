using System;
using System.IO;
using AddressProcessing.CSV.Exceptions;
using AddressProcessing.CSV.Models;

namespace AddressProcessing.CSV
{
    /*
        2) Refactor this class into clean, elegant, rock-solid & well performing code, without over-engineering.
           Assume this code is in production and backwards compatibility must be maintained.
    */


    /**
     * Comments on this task
     * 
     * I've tidied up the code and addressed the points raised in "CSVReaderWriter for Annotation.cs" with the exception of 
     * using async/await throughout as that would be a b/c breaking change if the methods in this file had the async keyword
     * added. I've also added more tests to increase test coverage
     * 
     * TODO
     * 1) Add a dependency injection contain like Unity to support constructor injection of dependencies and remove the need to 
     *    new up instances
     * 2) I would like to add some additional constructors to CsvReader() and CsvWriter() to allow a MemoryStrem to be passed in
     *    during testing. This would allow me to validate the use of StreamReader and StreamWriter more easily without needing 
     *    disk access
     * 
     */

    public class CSVReaderWriter : ICSVReaderWriter
    {
        private readonly ICsvReader _csvReader;
        private readonly ICsvWriter _csvWriter;

        [Flags]
        public enum Mode { Read = 1, Write = 2 };

        // Let's keep BC with those who call without a constructor
        public CSVReaderWriter()
        {
            _csvReader = new CsvReader();
            _csvWriter = new CsvWriter();
        }

        /// <summary>This would be used in the future to support dependency injection of the ICsvReader and ICsvWriter</summary>
        /// <param name="csvReader">An instance of ICsvReader</param>
        /// <param name="csvWriter">An instance of ICsvWriter</param>
        public CSVReaderWriter(ICsvReader csvReader, ICsvWriter csvWriter)
        {
            _csvReader = csvReader;
            _csvWriter = csvWriter;
        }

        /// <summary>Open a file for either reading or writing</summary>
        /// <param name="fileName">Path to the file</param>
        /// <param name="mode">An Mode enum value for reading or writing</param>
        /// <exception cref="UnsupportedFileAccessModeException">Thrown if a file mode other than Read or Write is used</exception>
        public void Open(string fileName, Mode mode)
        {
            if (mode == Mode.Read)
            {
                _csvReader.Open(fileName);
            }
            else if (mode == Mode.Write)
            {
                _csvWriter.Open(fileName);
            }
            else
            {
                // Throw a more specific exception here so that consumers can catch it and handle it as needed
                throw new UnsupportedFileAccessModeException("Unknown file mode for " + fileName);
            }
        }

        /// <summary>Write the columns to the CSV file</summary>
        /// <param name="columns">String array of columns</param>
        public void Write(params string[] columns)
        {
            // Formatting the input into the correct format for writing isn't the responsibility 
            // of this class. Instead that's been moved inside the CSV writer class
            _csvWriter.Write(columns);
        }

        /// <summary>Read a line from the CSV file</summary>
        /// <param name="column1">Out param for the value from column1</param>
        /// <param name="column2">Out param for the value from column2</param>
        /// <returns>true if a record was found, otherwise false</returns>
        public bool Read(out string column1, out string column2)
        {
            Contact customerContact;
            try
            {
                customerContact = _csvReader.ReadContactFromCsv().Result;
            }
            catch (Exception)
            {
                customerContact = null;
            }

            if (customerContact == null)
            {
                column1 = null;
                column2 = null;
                return false;
            }

            // A good thing to do here would be to have names like customerName and customerAddress as the out params
            // A better way would be to return an instance of Contact and let the consumer decide what data it needs 
            // from the object
            column1 = customerContact.Name;
            column2 = customerContact.Address;
            return true;
        }

        /// <summary>Close the ReaderWrite and clean up</summary>
        public void Close()
        {
            if (_csvWriter != null)
            {
                _csvWriter.Close();
                _csvWriter.Dispose();
            }

            if (_csvReader == null) return;
            _csvReader.Close();
            _csvReader.Dispose();
        }
    }
}
