using System;
using System.Linq;

namespace Cake.Arguments
{
    /// <summary>
    ///     The command argument name can be used to mark properties in the option class which
    ///     can then be dynamically looked up and assigned.
    /// </summary>
    /// <example>
    ///     <code>
    /// public class CommandOptions {
    ///     [CommandArgumentName("--title")]
    ///     [CommandArgumentName("-t")]
    ///     [CommandArgumentName("--page-header")]
    ///     public string Title { get; set;}
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    internal sealed class CommandArgumentNameAttribute : Attribute
    {
        /// <summary>
        ///     Gets the name of the command argument.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CommandArgumentNameAttribute" /> class.
        /// </summary>
        /// <param name="name">
        ///     The identifier of the command argument name.
        ///     It must start with either one or two dashes
        /// </param>
        /// <param name="oldStyle">
        ///     When set true a single char can use double dash and a long string can use a
        ///     single dash
        /// </param>
        public CommandArgumentNameAttribute(string name, bool oldStyle = false)
        {
            Name = name;

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("CommandArgumentName can not be empty or null", "name");
            }

            if (name.Any(char.IsWhiteSpace))
            {
                throw new ArgumentException("CommandArgumentName can not contain a space", "name");
            }

            var dashes = name.TakeWhile(x => x == '-').Count();
            if (dashes != 1 && dashes != 2)
            {
                throw new ArgumentException("CommandArgumentName must start with either one or two dashes", "name");
            }

            if (!(name.Length > dashes))
            {
                throw new ArgumentException("CommandArgumentName must have a name, only dashes is not allowed", "name");
            }

            // When using the old style the dash limitation doesn't apply.
            if (oldStyle)
            {
                return;
            }

            if (dashes == 1 && name.Length != 2)
            {
                throw new ArgumentException(
                    "CommandArgumentName with a single dash can only have one character following it", "name");
            }

            if (dashes == 2 && name.Length == 3)
            {
                throw new ArgumentException("CommandArgumentName with a double dash must use more than one character",
                    "name");
            }
        }
    }
}