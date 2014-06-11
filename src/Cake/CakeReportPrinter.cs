using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core;

namespace Cake
{
    internal static class CakeReportPrinter
    {
        public static void Write(CakeReport report)
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
