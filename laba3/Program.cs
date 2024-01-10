using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Xml.Serialization;

namespace lab3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CalendarService service = new CalendarService();
            Console.WriteLine(@"Usage:
Use one of commands:
""check"" to check is year leap
""calc"" to calc interval length
""day"" to get the name of day of week
""show"" to show all used dates
""quit"" to quit
""save"" to save
""load"" to load
");
            while (true)
            {
                string command = AskUser("Input the command:");
                switch (command)
                {
                    case "check":
                        int year = int.Parse(AskUser("Input the year"));
                        Console.Write($"Is year {year} leap? ");
                        Console.WriteLine(service.IsLeapYear(year));
                        break;
                    case "calc":
                        DateTime date1 = AskDateTime("first date");
                        DateTime date2 = AskDateTime("second date");
                        int length = service.CalcIntervalLength(date1, date2);
                        Console.WriteLine($"Length between two dates is {length}");
                        break;
                    case "day":
                        DateTime date = AskDateTime("for day of week calculation");
                        Console.WriteLine("This day is " + service.GetDayOfWeek(date));
                        break;
                    case "show":
                        Console.WriteLine("All used dates:");
                        Console.WriteLine(string.Join("\n", service.Dates));
                        break;
                    case "save":
                        string method = AskChoice("save method", ["Json", "Xml", "SQLite"]);
                        string path = AskUser("Saved file path");
                        switch (method)
                        {
                            case "Json":
                                File.WriteAllText(path, JsonSerializer.Serialize(service.Storage));
                                break;
                            case "Xml":
                                StreamWriter writer = new StreamWriter(path);
                                new XmlSerializer(typeof(CalendarStorage)).Serialize(writer, service.Storage);
                                break;
                            case "SQLite":
                                AppDbContext context = new AppDbContext(path);
                                context.Database.EnsureCreated();
                                context.Dates.ExecuteDelete();
                                context.Dates.AddRange(service.Storage.Dates);
                                context.SaveChanges();
                                break;
                        }
                        break;
                    case "load":
                        method = AskChoice("load method", ["Json", "Xml", "SQLite"]);
                        path = AskUser("Saved file path");
                        CalendarStorage storage = null;
                        switch (method)
                        {
                            case "Json":
                                storage = JsonSerializer.Deserialize<CalendarStorage>(File.ReadAllText(path))!;
                                break;
                            case "Xml":
                                StreamReader reader = new StreamReader(path);
                                storage = (CalendarStorage)new XmlSerializer(typeof(CalendarStorage)).Deserialize(reader)!;
                                break;
                            case "SQLite":
                                AppDbContext context = new AppDbContext(path);
                                context.Database.EnsureCreated();
                                storage = new CalendarStorage() { Dates = context.Dates.ToList() };
                                break;
                        }
                        service = new CalendarService(storage!);
                        break;
                    case "quit":
                        return;
                }
            }
        }

        static string AskUser(string text)
        {
            Console.WriteLine(text);
            return Console.ReadLine()!;
        }

        static DateTime AskDateTime(string text)
        {
            return new DateTime(int.Parse(AskUser($"Enter Year ({text})")),
                int.Parse(AskUser($"Enter Month ({text})")),
                int.Parse(AskUser($"Enter Day ({text})")));
        }

        static string AskChoice(string text, string[] choices)
        {
            while (true)
            {
                Console.WriteLine($"Select {text}");
                for (int i = 0; i < choices.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {choices[i]}");
                }
                int? result = int.Parse(Console.ReadLine()!);
                if (result.HasValue)
                {
                    if (0 < result && result <= choices.Length)
                    {
                        return choices[result.Value - 1];
                    }
                }
            }
        }
    }
}
