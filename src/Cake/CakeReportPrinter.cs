using System;
using System.Globalization;
using Cake.Core;

namespace Cake
{
    internal sealed class CakeReportPrinter : ICakeReportPrinter
    {
        public void Write(CakeReport report)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine();
                Console.WriteLine("{0,-30}{1,-20}", "Task", "Duration");
                Console.WriteLine(new string('-', 50));

                foreach (var item in report)
                {
                    var name = item.Key;
                    var time = item.Value.ToString("c", CultureInfo.InvariantCulture);
                    Console.WriteLine("{0,-30}{1,-20}", name, time);
                }
            }
            finally
            {
                Console.ResetColor();
            }
        }
    }
}
