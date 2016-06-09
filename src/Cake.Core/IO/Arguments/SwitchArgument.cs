// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Core.IO.Arguments
{
    /// <summary>
    /// Represents a argument preceded by a switch.
    /// </summary>
    public class SwitchArgument : IProcessArgument
    {
        private readonly string _switch;
        private readonly IProcessArgument _argument;
        private readonly string _separator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchArgument"/> class.
        /// </summary>
        /// <param name="switch">The switch.</param>
        /// <param name="argument">The argument.</param>
        /// <param name="separator">The separator between the <paramref name="switch"/> and the <paramref name="argument"/>.</param>
        public SwitchArgument(string @switch, IProcessArgument argument, string separator = " ")
        {
            _switch = @switch;
            _argument = argument;
            _separator = separator;
        }

        /// <summary>
        /// Render the arguments as a <see cref="System.String" />.
        /// Sensitive information will be included.
        /// </summary>
        /// <returns>
        /// A string representation of the argument.
        /// </returns>
        public string Render()
        {
            return string.Concat(_switch, _separator, _argument.Render());
        }

        /// <summary>
        /// Renders the argument as a <see cref="System.String" />.
        ///  The secret text will be redacted.
        /// </summary>
        /// <returns>A safe string representation of the argument.</returns>
        public string RenderSafe()
        {
            return string.Concat(_switch, _separator, _argument.RenderSafe());
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return RenderSafe();
        }
    }
}
