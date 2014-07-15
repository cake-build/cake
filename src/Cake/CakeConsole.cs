using System;
using Cake.Core;

namespace Cake
{
    internal sealed class CakeConsole : IConsole
    {
        public ConsoleColor ForegroundColor
        {
            get { return Console.ForegroundColor; }
            set { Console.ForegroundColor = value; }
        }

        public ConsoleColor BackgroundColor
        {
            get { return Console.BackgroundColor; }
            set { Console.BackgroundColor = value; }
        }

        public void Write(string format, params object[] arg)
        {
            Console.Write(format, arg);
        }

        public void WriteLine(string format, params object[] arg)
        {
            Console.WriteLine(format, arg);
        }

        public void ResetColor()
        {
            Console.ResetColor();
        }
    }
}
