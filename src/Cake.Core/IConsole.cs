using System;

namespace Cake.Core
{
    public interface IConsole
    {
        ConsoleColor ForegroundColor { get; set; }
        ConsoleColor BackgroundColor { get; set; }
        void Write(string format, params object[] arg);
        void WriteLine(string format, params object[] arg);
        void ResetColor();
    }
}
