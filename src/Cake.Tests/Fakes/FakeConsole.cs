using System;
using System.Collections.Generic;
using Cake.Core;

namespace Cake.Tests.Fakes
{
    public sealed class FakeConsole : IConsole
    {
        public List<string> Messages { get; set; }
        public ConsoleColor ForegroundColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }

        public FakeConsole()
        {
            Messages = new List<string>();
            ForegroundColor = ConsoleColor.Gray;
            BackgroundColor = ConsoleColor.Black;
        }

        public void Write(string format, params object[] arg)
        {
            Messages.Add(string.Format(format, arg));
        }

        public void WriteLine(string format, params object[] arg)
        {
            if (!string.IsNullOrWhiteSpace(format))
            {
                Messages.Add(string.Format(format, arg));   
            }            
        }

        public void ResetColor()
        {
            ForegroundColor = ConsoleColor.Gray;
            BackgroundColor = ConsoleColor.Black;
        }
    }
}
