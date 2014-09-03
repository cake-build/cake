using Cake.Core.IO;
using Cake.Core.IO.Arguments;

// ReSharper disable once CheckNamespace
namespace Cake.Core
{
    /// <summary>
    /// Contains extension methods for <see cref="ProcessArgumentBuilder"/>.
    /// </summary>
    public static class ProcessArgumentListExtensions
    {
        /// <summary>
        /// Appends the specified text to the argument builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="text">The text to be appended.</param>
        public static void Append(this ProcessArgumentBuilder builder, string text)
        {
            if (builder != null)
            {
                builder.Append(new TextArgument(text));
            }
        }

        /// <summary>
        /// Quotes and appends the specified text to the argument builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="text">The text to be appended.</param>
        public static void AppendQuoted(this ProcessArgumentBuilder builder, string text)
        {
            if (builder != null)
            {
                builder.Append(new QuotedArgument(new TextArgument(text)));
            }
        }

        /// <summary>
        /// Quotes and appends the specified argument to the argument builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="argument">The argument to be quoted and appended.</param>
        public static void AppendQuoted(this ProcessArgumentBuilder builder, ProcessArgument argument)
        {
            if (builder != null)
            {
                builder.Append(new QuotedArgument(argument));
            }
        }

        /// <summary>
        /// Appends the specified secret text to the argument builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="text">The secret text to be appended.</param>
        public static void AppendSecret(this ProcessArgumentBuilder builder, string text)
        {
            if (builder != null)
            {
                builder.Append(new SecretArgument(
                    new TextArgument(text)));
            }
        }

        /// <summary>
        /// Quotes and appends the specified secret text to the argument builder.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="text">The secret text to be quoted and appended.</param>
        public static void AppendQuotedSecret(this ProcessArgumentBuilder list, string text)
        {
            if (list != null)
            {
                list.AppendQuoted(new SecretArgument(
                    new TextArgument(text)));
            }
        }
    }
}
