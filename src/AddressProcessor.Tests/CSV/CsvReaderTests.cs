using System; 
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AddressProcessing.CSV;
using AddressProcessing.CSV.Exceptions;
using AddressProcessing.CSV.Models;
using NUnit.Framework;
using Moq;

namespace Csv.Tests
{
    [TestFixture]
    public class CsvReaderTests
    {

        private CsvReader _csvReader;

        [TestFixtureSetUp]
        public void Init()
        {
            _csvReader = new CsvReader();
        }


        [Test]
        public void TestThrowFileExceptionIfFileDoesntExist()
        {
            string fileName = GetTempFileName();
                
            Assert.Throws<UnableToOpenFileException>(delegate
            {
                _csvReader.Open(fileName);
            });
        }

        [Test]
        public void TestCallingWriteBeforeOpenThrowsException()
        {
            Assert.Throws<StreamReaderNotInitialisedException>(async delegate
            {
               await _csvReader.ReadLine();
            });
        }

        [Test]
        public void TestOpeningFileFromConstructorLoadsFile()
        {
            List<string[]> inputData = new List<string[]>()
            {
                new string[] { "Name1", "Address1"},
            };

            string filename = GenerateFileFromList(inputData);
            CsvReader csvReader = new CsvReader(filename);

            for (int i = 0; i < inputData.Count; i++)
            {
                Contact customer = csvReader.ReadContactFromCsv().Result;
                Assert.AreEqual(inputData[i][0], customer.Name);
                Assert.AreEqual(inputData[i][1], customer.Address);   
            }

        }

        [Test]
        public void TestLinesWithMissingColumnAreDiscarded()
        {
            List<string[]> inputData = new List<string[]>()
            {
                new string[] { "Name1", "Address1"},
                new string[] { "Name2"},
                new string[] { "Name3", "Address3"},
            };

            string filename = GenerateFileFromList(inputData);
            _csvReader.Open(filename);

            for (int i = 0; i < inputData.Count; i++)
            {
                Contact customer = _csvReader.ReadContactFromCsv().Result;
                if (inputData[i].Length > 1)
                {
                    Assert.AreEqual(inputData[i][0], customer.Name);
                    Assert.AreEqual(inputData[i][1], customer.Address);
                }
                else
                {
                    Assert.IsNull(customer);
                }
            }

        }


        private string GenerateFileFromList(List<string[]> inputFileData)
        {
            string fileName = GetTempFileName();

            StreamWriter tempFile = new StreamWriter(fileName);

            foreach (string[] lineArray in inputFileData)
            {
                String line = String.Join("\t", lineArray);   
                tempFile.WriteLine(line);
            }

            tempFile.Close();
            tempFile.Dispose();
            tempFile = null;

            return fileName;
        }

        private string GetTempFileName()
        {
            return Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        }


    }
}
