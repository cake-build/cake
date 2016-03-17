using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cake.Testing.Extensions
{
    /// <summary>
    /// Contains extensions for <see cref="FakeLog"/>.
    /// </summary>
    public static class FakeLogExtensions
    {
        /// <summary>
        /// Aggregates all current log entries message
        /// </summary>
        /// <param name="fakeLog">The fake log.</param>
        /// <returns>Log messages as <see cref="System.String"/></returns>
        public static string AggregateLogMessages(this FakeLog fakeLog)
        {
            return fakeLog.Entries.Aggregate(
                new StringBuilder(),
                (sb, entry) => sb.AppendLine(entry.Message),
                sb => sb.ToString());
        }
    }
}
