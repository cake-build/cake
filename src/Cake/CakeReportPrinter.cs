using System;
using System.Globalization;
using Cake.Core;

namespace Cake
{
    internal sealed class CakeReportPrinter : ICakeReportPrinter
    {
        private readonly IConsole _console;

        public CakeReportPrinter(IConsole console)
        {
            _console = console;
        }

        public void Write(CakeReport report)
        {
            if (report == null)
            {
                throw new ArgumentNullException("report");
            }

            try
            {
                _console.ForegroundColor = ConsoleColor.Green;
                _console.WriteLine();
                _console.WriteLine("{0,-30}{1,-20}", "Task", "Duration");
                _console.WriteLine(new string('-', 50));

                foreach (var item in report)
                {
                    var name = item.Key;
                    var time = item.Value.ToString("c", CultureInfo.InvariantCulture);
                    _console.WriteLine("{0,-30}{1,-20}", name, time);
                }
            }
            finally
            {
                _console.ResetColor();
            }
        }
    }
}
