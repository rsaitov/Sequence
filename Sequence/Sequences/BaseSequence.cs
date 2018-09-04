using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sequence
{
    public abstract class BaseSequence
    {
        private const string CounterLimitReached = "Итератор достиг максимального значения";
        private const string NumberLexemNotFound = "Формат последовательности не содержит итератора";
        private const string NumberLexemMoreThanOne = "Формат последовательности содержит больше одного итератора";

        protected int Item = 0;
        protected readonly List<ILexem> Lexems;

        protected BaseSequence(string format)
        {
            var formatParts = DivideFormatIntoParts(format);
            Lexems = CreateLexems(formatParts);
            ThrowExIfNotOneNumberLexem();
        }
        private static MatchCollection DivideFormatIntoParts(string format)
        {
            // *    -   Повторение 0 или более раз стоящего перед
            // .    -   Любой символ, кроме перевода строки или другого разделителя Unicode-строки
            // *?   -   Повторение 0 или более раз, минимально возможное количество
            // ()   -   Выделение в группы
            var pattern = Regex.Escape("[") + "(.*?)]";
            return Regex.Matches(format, pattern);
        }
        private List<ILexem> CreateLexems(MatchCollection formatParts)
        {
            var lexems = new List<ILexem>();
            foreach (var item in formatParts)
            {
                var stringObject = LexemFactory.CreateStringObject(item.ToString());
                lexems.Add(stringObject);
            }

            return lexems;
        }
        private void ThrowExIfNotOneNumberLexem()
        {
            var numberLexemCount = Lexems.Count(x => x.GetType() == typeof(NumberLexem));
            if (numberLexemCount == 0)
                throw new Exception(NumberLexemNotFound);
            if (numberLexemCount > 1)
                throw new Exception(NumberLexemMoreThanOne);
        }
        public void ThrowExIfIncrementLimitReached(int number)
        {
            var imitateIncrement = number + 1;
            var numberLexem = Lexems.First(x => x.GetType() == typeof(NumberLexem));
            var numberFormat = (numberLexem as NumberLexem).GetFormat();
            if (IncrementLimitReached(imitateIncrement, numberFormat))
                throw new Exception(CounterLimitReached);
        }
        private bool IncrementLimitReached(int number, string numberFormat)
        {
            return number.ToString().Length > numberFormat.Length;
        }
    }
}
