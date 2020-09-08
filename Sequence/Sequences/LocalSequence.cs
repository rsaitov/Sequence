using System.Text;

namespace Sequence
{
    
    public class LocalSequence : BaseSequence, ISequence
    {
        public LocalSequence(string format) : base(format)
        {

        }

        public object NextValue()
        {
            lock (Locker.IncrementLocker)
            {
                ThrowExIfIncrementLimitReached(Item);
                Item++;
                return Current();
            }
        }
        public object Current()
        {
            var output = new StringBuilder();            
            foreach (var lexem in Lexems)
            {
                output.Append(lexem.GetContent(Item));
            }

            return output.ToString();
        }
    }
}
