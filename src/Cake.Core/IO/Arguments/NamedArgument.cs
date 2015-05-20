namespace Cake.Core.IO.Arguments
{
    /// <summary>
    /// Represents a named argument and its value.
    /// </summary>
    public sealed class NamedArgument : IProcessArgument
    {
        private readonly string _name;
        private readonly IProcessArgument _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedArgument"/> class.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        public NamedArgument(string name)
            : this(name, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedArgument"/> class.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The argument value.</param>
        public NamedArgument(string name, IProcessArgument value)
        {
            _name = name;
            _value = value;
        }

        /// <summary>
        /// Render the arguments as a <see cref="string" />.
        /// </summary>
        /// <returns>A string representation of the argument.</returns>
        public string Render()
        {
            if (_value != null)
            {
                return "-" + _name + " " + _value.Render();
            }
            else
            {
                return "-" + _name;
            }
        }

        /// <summary>
        /// Renders the argument as a <see cref="string"/>.
        /// Sensitive information will be redacted.
        /// </summary>
        /// <returns>A safe string representation of the argument.</returns>
        public string RenderSafe()
        {
            if (_value != null)
            {
                return "-" + _name + " " + _value.RenderSafe();
            }
            else
            {
                return "-" + _name;
            }
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