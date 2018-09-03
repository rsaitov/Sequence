using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sequence.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SequenceTests
    {
        private ISequence _sequence;

        [TestMethod]
        public void CreateDbFile()
        {
            _sequence = new LocalSequence("[0]", @"db.txt");            
        }
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void FormatNoBrackets_Exception()
        {
            _sequence = new LocalSequence("0");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void FormatNotOneNumberLexemBrackets_Exception()
        {
            _sequence = new LocalSequence("[0][0]");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Format1DigitGenerate30_Exception()
        {
            _sequence = new LocalSequence("[0]");
            for (int i = 0; i < 30; i++)
                _sequence.NextValue();
        }

        [TestMethod]
        public void FormatIgnoreOutsideBrackets_ABC20180030()
        {
            _sequence = new LocalSequence("[ABC]x[year]y[0000]z");
            for (int i = 0; i < 30; i++)
                _sequence.NextValue();
            var current = _sequence.Current();
            Assert.AreEqual("ABC20180030", current);
        }

        [TestMethod]
        public void Start3ThreadsGenerateFormat_ABC20180090()
        {
            _sequence = new LocalSequence("[ABC][year][0000]");

            var tasks = new List<Task>();
            tasks.Add(Task.Factory.StartNew(GetTickets));
            tasks.Add(Task.Factory.StartNew(GetTickets));
            tasks.Add(Task.Factory.StartNew(GetTickets));
            Task.WaitAll(tasks.ToArray());

            var current = _sequence.Current();
            Assert.AreEqual("ABC20180090", current);
        }

        [TestMethod]
        public void Start5ThreadsGenerateFormat_ABC20180150()
        {
            _sequence = new LocalSequence("[ABC][year][0000]");

            var tasks = new List<Task>();
            tasks.Add(Task.Factory.StartNew(GetTickets));
            tasks.Add(Task.Factory.StartNew(GetTickets));
            tasks.Add(Task.Factory.StartNew(GetTickets));
            tasks.Add(Task.Factory.StartNew(GetTickets));
            tasks.Add(Task.Factory.StartNew(GetTickets));
            Task.WaitAll(tasks.ToArray());

            var current = _sequence.Current();
            Assert.AreEqual("ABC20180150", current);
        }

        private void GetTickets()
        {
            for (int i = 0; i < 30; i++)
                _sequence.NextValue();
        }
    }
}
