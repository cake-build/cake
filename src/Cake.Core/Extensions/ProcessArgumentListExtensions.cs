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
        public static T Append<T>(this T builder, string text) where T : IProcessArgumentList<T>
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
        public static T AppendQuoted<T>(this T builder, string text) where T : IProcessArgumentList<T>
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
        public static T AppendQuoted<T>(this T builder, IProcessArgument argument) where T : IProcessArgumentList<T>
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
        public static T AppendSecret<T>(this T builder, string text) where T : IProcessArgumentList<T>
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
        public static T AppendQuotedSecret<T>(this T builder, string text) where T : IProcessArgumentList<T>
        {
            if (builder != null)
            {
                builder.AppendQuoted(new SecretArgument(new TextArgument(text)));
            }

            return builder;
        }



        /// <summary>
        /// Appends the specified secret text to the argument builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="name">The argument name.</param>
        public static T AppendNamed<T>(this T builder, string name) where T : IProcessArgumentList<T>
        {
            if (builder != null)
            {
                builder.Append(new NamedArgument(name));
            }

            return builder;
        }

        /// <summary>
        /// Appends the specified secret text to the argument builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="name">The argument name.</param>
        /// <param name="text">The argument value.</param>
        public static T AppendNamed<T>(this T builder, string name, string text) where T : IProcessArgumentList<T>
        {
            if (builder != null)
            {
                builder.Append(new NamedArgument(name, new TextArgument(text)));
            }

            return builder;
        }

        /// <summary>
        /// Appends the specified secret text to the argument builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="name">The argument name.</param>
        /// <param name="text">The argument value.</param>
        public static T AppendNamedQuoted<T>(this T builder, string name, string text) where T : IProcessArgumentList<T>
        {
            if (builder != null)
            {
                builder.Append(new NamedArgument(name, new QuotedArgument(new TextArgument(text))));
            }

            return builder;
        }

        /// <summary>
        /// Quotes and appends the specified secret text to the argument builder.
        /// </summary>
        /// <param name="builder">The list.</param>
        /// <param name="name">The argument name.</param>
        /// <param name="text">The secret text to be quoted and appended.</param>
        public static T AppendNamedSecret<T>(this T builder, string name, string text) where T : IProcessArgumentList<T>
        {
            if (builder != null)
            {
                builder.AppendQuoted(new NamedArgument(name, new SecretArgument(new TextArgument(text))));
            }

            return builder;
        }

        /// <summary>
        /// Quotes and appends the specified secret text to the argument builder.
        /// </summary>
        /// <param name="builder">The list.</param>
        /// <param name="name">The argument name.</param>
        /// <param name="text">The secret text to be quoted and appended.</param>
        public static T AppendNamedQuotedSecret<T>(this T builder, string name, string text) where T : IProcessArgumentList<T>
        {
            if (builder != null)
            {
                builder.AppendQuoted(new NamedArgument(name, new QuotedArgument(new SecretArgument(new TextArgument(text)))));
            }

            return builder;
        }
    }
}
