using System;
using System.Collections.Generic;
using System.IO;

namespace Sequence.DbSet
{
    public class FileSet : IDbSet
    {
        private const string MoreThanOneSequenceFound = "Запись указанной последовательности в БД не уникальна";
        private string _fileName { get; set; }
        private string _format { get; set; }
        private ContextSequence _sequence { get; set; }
        private FileStream fsWriter { get; set; }
        public FileSet(string format, string fileName, ContextSequence sequence)
        {
            _fileName = fileName;
            _format = format;
            _sequence = sequence;
        }
        public void CreateIfNotExists()
        {
            if (!File.Exists(_fileName))
            {
                FileStream fs = File.Create(_fileName);
                fs.Close();
            }
        }
        public void SetSequenceIfNotExists()
        {
            using (FileStream fs = new FileStream(_fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                var exists = false;
                var messageOnSuccess = "";

                var sw = new StreamWriter(fs);
                var sr = new StreamReader(fs);

                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    var parts = line.Split('\t');
                    if (parts[0] == this._format && parts.Length > 1)
                    {
                        if (exists)
                            throw new Exception(MoreThanOneSequenceFound);

                        messageOnSuccess = string.Format("Найдена последовательность {0}. Последнее значение = {1}", _format, parts[1]);
                        exists = true;                        
                    }
                }

                if (!exists)
                    sw.WriteLine(string.Format("{0}\t{1}", this._format, 0));

                Console.WriteLine(messageOnSuccess);
                sw.Close();
            }
        }

        public int NextValue()
        {
            using (var fsReader = new FileStream(_fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                var lines = ReadAllLines(fsReader);
                File.WriteAllText(_fileName, "");

                var nextValue = -1;
                var exceptionMessage = "";

                using (fsWriter = new FileStream(_fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
                {
                    var sw = new StreamWriter(fsWriter);
                    nextValue = CheckEmptyLines(lines, nextValue, sw);

                    foreach (var line in lines)
                    {
                        var parts = line.Split('\t');
                        if (parts[0] == this._format && parts.Length > 1)
                        {
                            var currentValue = Convert.ToInt32(parts[1]);
                            try
                            {
                                _sequence.ThrowExIfIncrementLimitReached(currentValue);
                                nextValue = currentValue + 1;
                            }
                            catch (Exception e)
                            {
                                nextValue = currentValue;
                                exceptionMessage = e.Message;
                            }

                            sw.WriteLine(string.Format("{0}\t{1}", this._format, nextValue));
                        }
                        else
                            sw.WriteLine(line);
                    }

                    sw.Close();
                    fsWriter.Close();
                }

                if (!string.IsNullOrEmpty(exceptionMessage))
                    throw new Exception(exceptionMessage);

                fsReader.Close();
                return nextValue;
            }
        }
        private static List<string> ReadAllLines(FileStream fs)
        {
            List<string> lines = new List<string>();
            var sr = new StreamReader(fs);
            while (!sr.EndOfStream)
                lines.Add(sr.ReadLine());
            sr.Close();
            return lines;
        }
        private int CheckEmptyLines(List<string> lines, int nextValue, StreamWriter sw)
        {
            if (lines.Count == 0)
            {
                nextValue = 1;
                sw.WriteLine(string.Format("{0}\t{1}", this._format, nextValue));
            }

            return nextValue;
        }
    }
}
