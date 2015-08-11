using System.Collections.Generic;
using System.IO;

// ReSharper disable once CheckNamespace
namespace Cake.Tests
{
    public static class StreamReaderExtensions
    {
        public static IEnumerable<string> ReadAllLines(this StreamReader reader)
        {
            var result = new List<string>();
            while (true)
            {
                var line = reader.ReadLine();
                if (line == null)
                {
                    break;
                }
                result.Add(line);
            }
            return result;
        }
    }
}
