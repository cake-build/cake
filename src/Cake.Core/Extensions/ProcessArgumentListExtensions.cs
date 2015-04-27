using Cake.Core.IO;
using Cake.Core.IO.Arguments;

// ReSharper disable once CheckNamespace
namespace Cake.Core
{
    /// <summary>
    /// Contains extension methods for <see cref="ProcessArgumentBuilder" />.
    /// </summary>
    public static class ProcessArgumentListExtensions
    {
        /// <summary>
        /// Appends the specified text to the argument builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="text">The text to be appended.</param>
        /// <returns>The same <see cref="ProcessArgumentBuilder"/> instance so that multiple calls can be chained.</returns>
        public static ProcessArgumentBuilder Append(this ProcessArgumentBuilder builder, string text)
        {
            if (builder != null)
            {
                builder.Append(new TextArgument(text));
            }
            return builder;
        }

        /// <summary>
        /// Quotes and appends the specified text to the argument builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="text">The text to be appended.</param>
        /// <returns>The same <see cref="ProcessArgumentBuilder"/> instance so that multiple calls can be chained.</returns>
        public static ProcessArgumentBuilder AppendQuoted(this ProcessArgumentBuilder builder, string text)
        {
            if (builder != null)
            {
                builder.Append(new QuotedArgument(new TextArgument(text)));
            }
            return builder;
        }

        /// <summary>
        /// Quotes and appends the specified argument to the argument builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="argument">The argument to be quoted and appended.</param>
        /// <returns>The same <see cref="ProcessArgumentBuilder"/> instance so that multiple calls can be chained.</returns>
        public static ProcessArgumentBuilder AppendQuoted(this ProcessArgumentBuilder builder, IProcessArgument argument)
        {
            if (builder != null)
            {
                builder.Append(new QuotedArgument(argument));
            }
            return builder;
        }

        /// <summary>
        /// Appends the specified secret text to the argument builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="text">The secret text to be appended.</param>
        /// <returns>The same <see cref="ProcessArgumentBuilder"/> instance so that multiple calls can be chained.</returns>
        public static ProcessArgumentBuilder AppendSecret(this ProcessArgumentBuilder builder, string text)
        {
            if (builder != null)
            {
                builder.Append(new SecretArgument(new TextArgument(text)));
            }
            return builder;
        }

        /// <summary>
        /// Quotes and appends the specified secret text to the argument builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="text">The secret text to be quoted and appended.</param>
        /// <returns>The same <see cref="ProcessArgumentBuilder"/> instance so that multiple calls can be chained.</returns>
        public static ProcessArgumentBuilder AppendQuotedSecret(this ProcessArgumentBuilder builder, string text)
        {
            if (builder != null)
            {
                builder.AppendQuoted(new SecretArgument(new TextArgument(text)));
            }
            return builder;
        }
    }
}
