using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sequence;

namespace MainProgram
{
    class Program
    {
        private static ISequence _sequence;

        static void Main(string[] args)
        {
            ClientCodeContextSequence2();
        }
        private static void ClientCodeLocalSequence()
        {
            try
            {
                Console.WriteLine(@"Запускаем 3 процесса, в каждом из которых получаем 10 тикетов...");
                Console.WriteLine(@"Выдача тикетов синхронизирована, порядок вывода на экран - нет,");
                Console.WriteLine(@"т.к. очерёдность получения тикетов в процессах рандомная.");
                Console.WriteLine("");

                _sequence = new LocalSequence("[ABC_][0000][_][year]");

                Console.WriteLine();
                Console.WriteLine(@"Выданные тикеты:");
                Console.WriteLine();
                var task1 = Task<List<string>>.Factory.StartNew(GetTickets);
                var task2 = Task<List<string>>.Factory.StartNew(GetTickets);
                var task3 = Task<List<string>>.Factory.StartNew(GetTickets);
                var tasks = new List<Task>() {task1, task2, task3};

                Console.WriteLine("Процесс печатает тикет сразу после его получения.");
                Console.WriteLine("Один из процессов может получить тикет позже, но успеть напечатать его раньше.");
                Console.WriteLine("Поэтому порядок отображения может быть нарушен.");
                Console.WriteLine("");
                Task.WaitAll(tasks.ToArray());

                var result1 = task1.Result;
                var result2 = task2.Result;
                var result3 = task3.Result;

                Console.WriteLine("");
                Console.Write("Тикеты №1 \t Тикеты №2 \t Тикеты №3");
                Console.WriteLine("");
                for (int i = 0; i < 10; i++)
                {
                    Console.Write(result1[i] + " \t " + result2[i] + " \t " + result3[i]);
                    Console.WriteLine("");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();
        }
        private static List<string> GetTickets()
        {
            var list = new List<string>();
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    var value = _sequence.NextValue();
                    Console.Write(value + "\n");
                    list.Add(value.ToString());                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return list;
        }
        private static void ClientCodeContextSequence1()
        {
            try
            {
                _sequence = new ContextSequence("[0]", @"db.txt");
                for (int i = 0; i < 11; i++)
                {
                    var value = _sequence.NextValue();
                    Console.Write(value + "\n");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();
        }
        private static void ClientCodeContextSequence2()
        {
            try
            {
                _sequence = new ContextSequence("[ABC_][0000][_][year]", @"db.txt");

                Console.WriteLine();
                Console.WriteLine(@"Выданные тикеты:");
                Console.WriteLine();
                var task1 = Task<List<string>>.Factory.StartNew(GetTicketsWithRandomTimeIntervals);
                var task2 = Task<List<string>>.Factory.StartNew(GetTicketsWithRandomTimeIntervals);
                var task3 = Task<List<string>>.Factory.StartNew(GetTicketsWithRandomTimeIntervals);
                var tasks = new List<Task>() { task1, task2, task3 };

                Task.WaitAll(tasks.ToArray());

                var result1 = task1.Result;
                var result2 = task2.Result;
                var result3 = task3.Result;

                Console.WriteLine("");
                Console.Write("Тикеты №1 \t Тикеты №2 \t Тикеты №3");
                Console.WriteLine("");
                for (int i = 0; i < 10; i++)
                {
                    Console.Write(result1[i] + " \t " + result2[i] + " \t " + result3[i]);
                    Console.WriteLine("");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();
        }
        private static List<string> GetTicketsWithRandomTimeIntervals()
        {
            var list = new List<string>();
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    var value = _sequence.NextValue();
                    Console.Write(value + "\n");
                    list.Add(value.ToString());
                    Thread.Sleep(new Random().Next(100, 200));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return list;
        }
    }
}
