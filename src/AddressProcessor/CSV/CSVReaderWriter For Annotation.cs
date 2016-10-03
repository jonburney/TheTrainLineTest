using System;
using System.IO;

namespace AddressProcessing.CSV
{
    /*
        1) List three to five key concerns with this implementation that you would discuss with the junior developer. 

        Please leave the rest of this file as it is so we can discuss your concerns during the next stage of the interview process.
        
        i) SOLID - There are a few specific issues relating to SOLID principles. Generally speaking, code that is built with SOLID principles is 
           easier to build upon and maintain. 
           
             a) Single responsibility - The CSVReaderWriter is doing too much. It's responsible for reading from disk and writing to disk, and also data 
                 manipulation to get data into the correct format. Instead there should be a separate class for reading data and for writing data 
                 (encapsulating their own data manipulation code). That way if you wanted a read-only consumer then you can use just the CsvReader class.
             b) Dependency inversion principle - Right now lots of the code relies on concrete types that are instantiated with the 'new' keyword. This
                 means that you can't easily substitute one of these objects for another subtype. An improvement would be to implement an interface and pass these
                 interfaces into the constructors of the objects. This way you can code against an abstraction without knowing the exact details of the 
                 implementation. This will also make it easier to test since you can pass in a mock object that implements the same interface and use the
                 mock to validate your code in a unit test. 
             c) Interface segregation principle - An extension of the above is that rather than just having a single large interface you should use smaller
                 specific interfaces. An example would be an ICsvReader interface for read operations and ICsvWriter for write operations. Classes can implement
                 both, or just one, or contain sub-objects that implement each.
        ii) The implementation lacks a use of strong types. Right now the CSVReaderWriter() has out params defined as strings. A better approach would be
            to define a specific Contact() class that is returned. The consumer can then access the customer name and address from this object. There is
            an AddressRecord() class in the project already so the Contact() class could make use of this to hold the address data. Right now this would be a 
            breaking change because the consumers expect strings to be returned. It's hard to know exactly what data you are dealing with if you just have
            strings called column1 and column2 (is that name and address or address and name etc). Right now the consumer (AddressFileProcessor) has to know
            the implementation of the underlying data and understand that column1 is name and column2 is address. Having a Contact() object would make it much
            clearer
        ii) The file handling code could be improved. 
             a) AddressFileProcessor() has to understand that CSVReaderWriter() is using StreamReader and StreamWriter and has to call CSVReaderWriter.Close() 
                when it's finished. This is an implementation detail that isn't necessary to expose. 
             b) Both StreamReader and StreamWriter implement the IDisposable interface so we can make use of that to handle the disposal of the objects when we 
                are done. We could do this via a 'using' statement if the code was refactored.
             c) Blocking threads during I/O. The code currently uses StreamReader.ReadLine() and StreamWriter.WriteLine(). This is accessing the file on disk
                and during this time the thread is waiting for data to come back blocking other processes from using the thread. If disk I/O is slow then the 
                thread may be sat waiting idly. Threads are expensive to spin up so there is a pool of them available to .NET processes, called a ThreadPool. 
                When you perform any kind of I/O (file access, network access, HTTP requests etc) you should really use Async/Await. This will return the thread
                to the threadpool so that other processes can use it while it waits for the I/O operation to complete, it will then jump back into your code when
                the operation completes. In this example we could use the Async methods StreamReader.ReadLineAsync() and StreamWriter.WriteLineAsync() to avoid 
                blocking threads. To implement this fully we need to use async/await all the way up through the code (including in AddressFileProcessor) which 
                would be a breaking change.
    */

    public class CSVReaderWriterForAnnotation
    {
        private StreamReader _readerStream = null;
        private StreamWriter _writerStream = null;

        [Flags]
        public enum Mode { Read = 1, Write = 2 };

        public void Open(string fileName, Mode mode)
        {
            if (mode == Mode.Read)
            {
                _readerStream = File.OpenText(fileName);
            }
            else if (mode == Mode.Write)
            {
                FileInfo fileInfo = new FileInfo(fileName);
                _writerStream = fileInfo.CreateText();
            }
            else
            {
                throw new Exception("Unknown file mode for " + fileName);
            }
        }

        public void Write(params string[] columns)
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

            WriteLine(outPut);
        }

        public bool Read(string column1, string column2)
        {
            const int FIRST_COLUMN = 0;
            const int SECOND_COLUMN = 1;

            string line;
            string[] columns;

            char[] separator = { '\t' };

            line = ReadLine();
            columns = line.Split(separator);

            if (columns.Length == 0)
            {
                column1 = null;
                column2 = null;

                return false;
            }
            else
            {
                column1 = columns[FIRST_COLUMN];
                column2 = columns[SECOND_COLUMN];

                return true;
            }
        }

        public bool Read(out string column1, out string column2)
        {
            const int FIRST_COLUMN = 0;
            const int SECOND_COLUMN = 1;

            string line;
            string[] columns;

            char[] separator = { '\t' };

            line = ReadLine();

            if (line == null)
            {
                column1 = null;
                column2 = null;

                return false;
            }

            columns = line.Split(separator);

            if (columns.Length == 0)
            {
                column1 = null;
                column2 = null;

                return false;
            } 
            else
            {
                column1 = columns[FIRST_COLUMN];
                column2 = columns[SECOND_COLUMN];

                return true;
            }
        }

        private void WriteLine(string line)
        {
            _writerStream.WriteLine(line);
        }

        private string ReadLine()
        {
            return _readerStream.ReadLine();
        }

        public void Close()
        {
            if (_writerStream != null)
            {
                _writerStream.Close();
            }

            if (_readerStream != null)
            {
                _readerStream.Close();
            }
        }
    }
}
