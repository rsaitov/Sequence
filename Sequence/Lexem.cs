using System;

namespace Sequence
{
    public interface ILexem
    {
        string GetContent(int number);
    }
    public class SymbolsLexem : ILexem
    {
        private readonly string _letters;

        public string GetContent(int number)
        {
            return _letters;
        }

        public SymbolsLexem(string obj)
        {
            _letters = obj;
        }
    }
    public class YearLexem : ILexem
    {
        public string GetContent(int number)
        {
            return DateTime.Now.Year.ToString();
        }
    }
    public class NumberLexem : ILexem
    {
        private readonly string _format;
        public string GetContent(int number)
        {
            return number.ToString(_format);
        }
        public NumberLexem(string obj)
        {
            _format = obj;
        }

        public string GetFormat()
        {
            return _format;
        }
    }
    public static class LexemFactory
    {
        public static ILexem CreateStringObject(string obj)
        {
            obj = obj.Replace("[", "").Replace("]", "");
            if (CheckIfConsistOnlyOfNulls(obj))
                return new NumberLexem(obj);

            switch (obj.ToLower())
            {
                case "year":
                    return new YearLexem();
                default:
                    return new SymbolsLexem(obj);
            }
        }

        public static bool CheckIfConsistOnlyOfNulls(string obj)
        {
            return obj.Replace("0", "").Length == 0 ? true : false;
        }
    }
}
