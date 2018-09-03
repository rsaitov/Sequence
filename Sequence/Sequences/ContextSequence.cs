using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sequence.DbSet;

namespace Sequence
{
    public class ContextSequence : BaseSequence, ISequence
    {
        private IDbSet _context;

        public ContextSequence(string format, string fileName = null) : base(format)
        {
            _context = new FileSet(format, fileName, this);
            _context.CreateIfNotExists();
            _context.SetSequenceIfNotExists();
        }

        public object NextValue()
        {
            lock (Locker.IncrementLocker)
            {                
                Item = _context.NextValue();
                return Current();
            }
        }
        public object Current()
        {
            string output = "";
            foreach (var lexem in Lexems)
            {
                output += lexem.GetContent(Item);
            }

            return output;
        }
    }
}
