// ReSharper disable once CheckNamespace
namespace Cake.Core.Tests
{
    public static class StringExtensions
    {
        /// <summary>
        /// Removes line endings from the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The text without line ending</returns>
        public static string NormalizeGeneratedCode(this string text)
        {
            return text.NormalizeLineEndings()
                .TrimEnd(new[] { '\r', '\n' });
        }
    }
}
