using System;
using System.Collections;
using System.Collections.Generic;
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
    public class CSVReaderWriterTests
    {

        private CSVReaderWriter _csvReaderWriter;
        private Mock<ICsvReader> _mockCsvReader;
        private Mock<ICsvWriter> _mockCsvWriter;

        [TestFixtureSetUp]
        public void Init()
        {
            _mockCsvReader   = new Mock<ICsvReader>();
            _mockCsvWriter   = new Mock<ICsvWriter>();
            _csvReaderWriter = new CSVReaderWriter(_mockCsvReader.Object, _mockCsvWriter.Object);
        }

        [Test]
        public void TestCallingCloseCleansUpCorrectly()
        {
            _mockCsvReader.Setup(x => x.Close());
            _mockCsvReader.Setup(x => x.Dispose());

            _mockCsvWriter.Setup(x => x.Close());
            _mockCsvWriter.Setup(x => x.Dispose());

            _csvReaderWriter.Close();

            _mockCsvReader.Verify(x => x.Close(), Times.Once);
            _mockCsvReader.Verify(x => x.Dispose(), Times.Once);
            _mockCsvWriter.Verify(x => x.Close(), Times.Once);
            _mockCsvWriter.Verify(x => x.Dispose(), Times.Once);
        }

        [Test]
        public void TestCorrectNumberOfRecordsReturnedFromRead()
        {
            List<Contact> fakeContacts = new List<Contact>()
            {
                new Contact() {Name = "Name1", Address = "Address1"},
                new Contact() {Name = "Name2", Address = "Address2"},
                new Contact() {Name = "Name3", Address = "Address3"}
            };

            Queue<Contact> fakeContactsQueue = new Queue<Contact>(fakeContacts);

            _mockCsvReader.Setup(x => x.ReadContactFromCsv()).Returns(() => Task.FromResult(fakeContactsQueue.Dequeue()));

            string column1, column2;
            int counter = 0;

            while (_csvReaderWriter.Read(out column1, out column2))
            {
                Assert.AreEqual(fakeContacts[counter].Name, column1);
                Assert.AreEqual(fakeContacts[counter].Address, column2);
                counter++;
            }
            
            Assert.AreEqual(fakeContacts.Count, counter);
        }

        [Test]
        public void TestFalseIsReturnedWhenThereIsNoMoreDataToRead()
        {
            _mockCsvReader.Setup(x => x.ReadContactFromCsv()).ReturnsAsync(null);

            string column1, column2;
            Assert.IsFalse(_csvReaderWriter.Read(out column1, out column2));
            Assert.IsNullOrEmpty(column1);
            Assert.IsNullOrEmpty(column2);
        }

        [Test]
        public void TestFileOpenWithUnSupportedModeThrowsException()
        {
            Assert.Throws<UnsupportedFileAccessModeException>(
                delegate
                {
                    _csvReaderWriter.Open("someFile", (CSVReaderWriter.Mode)4);
                }
            ); 
                
        }

    }
}
