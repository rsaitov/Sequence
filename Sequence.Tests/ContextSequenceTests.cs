using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Sequence.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ContextSequenceTests
    {
        private ISequence _sequence;

        [TestMethod]
        public void NoDbFile_Success()
        {
            File.Delete(@"db_test.txt");
            CreateSequenceWithFormat("[0]");
        }
        [TestMethod]
        public void CreateDbFile_Success()
        {
            DeleteAndCreateDbFileWithContent("[0]\t5\r\n");
            CreateSequenceWithFormat("[0]");
        }
        [TestMethod]
        public void CreateDbFileSeveralSequencesInFile_Success()
        {
            DeleteAndCreateDbFileWithContent("[ABC][00]\t5\r\n[0]\t5\r\n");
            CreateSequenceWithFormat("[0]");
            _sequence.NextValue();
        }

        [TestMethod]
        public void CreateDbFile_SequenceNotFound()
        {
            DeleteAndCreateDbFileWithContent("[00]\t5\r\n");
            CreateSequenceWithFormat("[0]");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CreateDbFile_ExceptionMoreThanOneSequenceFound()
        {
            DeleteAndCreateDbFileWithContent("[0]\t5\r\n[0]\t5\r\n");
            CreateSequenceWithFormat("[0]");
        }

        [TestMethod]
        public void Format3DigitGenerate5_Success()
        {
            DeleteAndCreateDbFileWithContent("[0]\t3\r\n");
            CreateSequenceWithFormat("[0]");

            for (int i = 0; i < 5; i++)
                _sequence.NextValue();
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Format1DigitGenerate10_ExceptionLimitReached()
        {
            DeleteAndCreateDbFileWithContent("[0]\t0\r\n");
            CreateSequenceWithFormat("[0]");
            for (int i = 0; i < 10; i++)
                _sequence.NextValue();
        }

        [TestMethod]
        public void EmptyFileDuringWorking_Success()
        {
            DeleteAndCreateDbFileWithContent("[0]\t5\r\n");
            CreateSequenceWithFormat("[0]");
            DeleteAndCreateDbFileWithContent("");
            _sequence.NextValue();
        }

        private void DeleteAndCreateDbFileWithContent(string content)
        {
            File.Delete(@"db_test.txt");
            File.WriteAllText(@"db_test.txt", content);
        }
        private void CreateSequenceWithFormat(string format)
        {
            _sequence = new ContextSequence(format, @"db_test.txt");
        }
    }
}
