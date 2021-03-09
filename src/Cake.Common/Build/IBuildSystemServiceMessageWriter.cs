using System;

namespace Cake.Common.Build
{
    /// <summary>
    /// Represents build system output.
    /// </summary>
    public interface IBuildSystemServiceMessageWriter
    {
        /// <summary>
        /// Writes a message to the build system output.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using format.</param>
        void Write(string format, params object[] args);
    }

    internal sealed class BuildSystemServiceMessageWriter : IBuildSystemServiceMessageWriter
    {
        public void Write(string format, params object[] args)
        {
            Console.Out.WriteLine(format, args);
            Console.Out.Flush();
        }
    }
}
