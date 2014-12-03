using System.Text;

namespace Cake.Common.Tests.Extensions
{
    internal static class StringBuilderExtensions
    {
        internal static void AppendFormatLine(this StringBuilder builder, string format, params object[] args)
        {
            builder.AppendLine(string.Format(format, args));
        }
    }
}
