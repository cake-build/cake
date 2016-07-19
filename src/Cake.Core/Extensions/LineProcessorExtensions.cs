using System;
using Cake.Core.Scripting.Processors;

namespace Cake.Core.Extensions
{
    /// <summary>
    /// Contains the extension methods for a <see cref="LineProcessor"/>.
    /// </summary>
    public static class LineProcessorExtensions
    {
        /// <summary>
        /// Split line on Whitespace.
        /// </summary>
        /// <param name="processor">The line processor.</param>
        /// <param name="line">The script line.</param>
        /// <returns>A string array containing the split result</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string[] Split(this ILineProcessor processor, string line)
        {
            return line.SplitLine();
        }
    }
}
