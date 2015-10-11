// ReSharper disable once CheckNamespace
namespace Cake.Core
{
    /// <summary>
    /// Contains extension methods for <see cref="IConsole"/>.
    /// </summary>
    public static class ConsoleExtensions
    {
        /// <summary>
        /// Writes an empty line to the console output.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        public static void WriteLine(this IConsole console)
        {
            if (console != null)
            {
                console.WriteLine(string.Empty);
            }
        }
    }
}