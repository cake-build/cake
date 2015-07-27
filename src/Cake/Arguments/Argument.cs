namespace Cake.Arguments
{
    internal struct Argument
    {
        /// <summary>
        /// Gets the key for this argument.
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// Gets the string value for this argument.
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Gets the position in which the argument appeared.
        /// </summary>
        public uint Position { get; private set; }
       
        /// <summary>
        /// Initializes a new instance of the <see cref="Argument"/> struct.
        /// </summary>
        /// <param name="key">The key for this argument, or blank if this is a positional argument.</param>
        /// <param name="value">The value of the argument.</param>
        /// <param name="position">The position of the argument.</param>
        public Argument(string key, string value, uint position)
        {
            Key = key;
            Value = value;
            Position = position;
        }
    }
}
