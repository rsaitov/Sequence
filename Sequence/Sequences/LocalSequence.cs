
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
            string output = "";
            foreach (var lexem in Lexems)
            {
                output += lexem.GetContent(Item);
            }

            return output;
        }
    }
}
