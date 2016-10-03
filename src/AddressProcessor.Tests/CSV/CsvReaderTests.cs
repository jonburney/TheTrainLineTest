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
        public void TestCallingWriteBeforeOpenThrowsException()
        {
            Assert.Throws<StreamReaderNotInitialisedException>(async delegate
            {
               await _csvReader.ReadLine();
            });
        }
    }
}
