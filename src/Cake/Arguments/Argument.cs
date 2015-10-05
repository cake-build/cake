﻿namespace Cake.Arguments
{
    internal class Argument
    {
        /// <summary>
        /// Gets or sets the key for this argument.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the string value for this argument.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the position in which the argument appeared.
        /// </summary>
        public int Position { get; set; }
    }
}
