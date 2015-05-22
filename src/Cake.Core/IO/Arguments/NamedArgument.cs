using System;
using System.Globalization;

namespace Cake.Core.IO.Arguments
{
    /// <summary>
    /// Represents a named argument and its value.
    /// </summary>
    public sealed class NamedArgument : IProcessArgument
    {
        private readonly string _name;
        private readonly IProcessArgument _value;

        private string _format;

        /// <summary>
        /// The default format for named arguments
        /// </summary>
        public const string DefaultFormat = "-{0} {1}";

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedArgument"/> class.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The argument value.</param>
        public NamedArgument(string name, IProcessArgument value)
            : this(name, value, NamedArgument.DefaultFormat)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedArgument"/> class.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The argument value.</param>
        /// <param name="format">The format of argument.</param>
        public NamedArgument(string name, IProcessArgument value, string format)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (string.IsNullOrEmpty(format))
            {
                throw new ArgumentNullException("format");
            }

            _name = name;
            _value = value;
            _format = format;
        }

        /// <summary>
        /// Gets or sets the format of the argument
        /// </summary>
        /// <value>The argument format.</value>
        public string Format
        {
            get
            {
                return _format;
            }

            set
            {
                _format = value;
            }
        }

        /// <summary>
        /// Render the arguments as a <see cref="string" />.
        /// </summary>
        /// <returns>A string representation of the argument.</returns>
        public string Render()
        {
            return string.Format(CultureInfo.InvariantCulture, _format, _name, _value.Render());
        }

        /// <summary>
        /// Renders the argument as a <see cref="string"/>.
        /// Sensitive information will be redacted.
        /// </summary>
        /// <returns>A safe string representation of the argument.</returns>
        public string RenderSafe()
        {
            return string.Format(CultureInfo.InvariantCulture, _format, _name, _value.RenderSafe());
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.RenderSafe();
        }
    }
}