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
    public class CsvWriterTests
    {

        private CsvWriter _csvWriter;

        [TestFixtureSetUp]
        public void Init()
        {
            _csvWriter = new CsvWriter();
        }


        [Test]
        public void TestWritingLineHandlesMultipleColumns()
        {
            string fileName = GetTempFileName();
            _csvWriter.Open(fileName);

            List<string[]> linevalues = new List<string[]>()
            {
                new string[] { "Column 1" },
                new string[] { "Column 1", "Column 2" },
                new string[] { "Column 1", "Column 2", "Column 3" },
            };

            foreach (string[] line in linevalues)
            {
                _csvWriter.Write(line);
            }

            _csvWriter.Close();

            StreamReader reader = new StreamReader(fileName);

            for (int i = 0; i < linevalues.Count; i++)
            {
                string lineValue = reader.ReadLine();
                string[] lineArray = lineValue.Split('\t');

                Assert.AreEqual(linevalues[i].Length, lineArray.Length);
                for (int j = 0; j < linevalues[i].Length; j++)
                {
                    Assert.AreEqual(linevalues[i][j], lineArray[j]);
                }
            }
            
            reader.Close();
            reader.Dispose();
            reader = null;
        }
        

        [Test]
        public void TestCallingWriteBeforeOpenThrowsException()
        {
            Assert.Throws<StreamWriterNotInitialisedException>(async delegate
            {
               _csvWriter.WriteLine("This Is a Test");
            });
        }

        private string GetTempFileName()
        {
            return Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        }



    }
}
