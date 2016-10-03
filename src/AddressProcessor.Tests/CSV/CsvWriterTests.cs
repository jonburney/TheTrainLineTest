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
        public void TestCallingWriteBeforeOpenThrowsException()
        {
            Assert.Throws<StreamWriterNotInitialisedException>(async delegate
            {
               await _csvWriter.WriteLine("This Is a Test");
            });
        }
    }
}
