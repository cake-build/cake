#region Using Statements
    using System;
    using System.Security;

    using Cake.Core.IO;
#endregion



namespace Cake.Core.IO.Arguments
{
    /// <summary>
    /// Represents a named argument and its value.
    /// </summary>
    public sealed class NamedArgument : IProcessArgument
    {
        private readonly string _Name;
        private readonly IProcessArgument _Value;



        /// <summary>
        /// Initializes a new instance of the <see cref="TextArgument"/> class.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        public NamedArgument(string name)
            : this(name, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextArgument"/> class.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The argument value.</param>
        public NamedArgument(string name, IProcessArgument value)
        {
            _Name = name;
            _Value = value;
        }



        /// <summary>
        /// Render the arguments as a <see cref="string" />.
        /// </summary>
        /// <returns>A string representation of the argument.</returns>
        public string Render()
        {
            if (_Value != null)
            {
                return "-" + _Name + " " + _Value.Render();
            }
            else
            {
                return "-" + _Name;
            }
        }

        /// <summary>
        /// Renders the argument as a <see cref="string"/>.
        /// Sensitive information will be redacted.
        /// </summary>
        /// <returns>A safe string representation of the argument.</returns>
        public string RenderSafe()
        {
            if (_Value != null)
            {
                return "-" + _Name + " " + _Value.RenderSafe();
            }
            else
            {
                return "-" + _Name;
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
