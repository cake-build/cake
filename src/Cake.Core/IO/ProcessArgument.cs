namespace Cake.Core.IO
{
    /// <summary>
    /// Represents a process argument.
    /// </summary>
    public abstract class ProcessArgument
    {
        /// <summary>
        /// Render the arguments as a <see cref="string"/>.
        /// Sensitive information will be included.
        /// </summary>
        /// <returns>A string representation of the argument.</returns>
        public abstract string Render();

        /// <summary>
        /// Renders the argument as a <see cref="string"/>.
        /// Sensitive information will be redacted.
        /// </summary>
        /// <returns>A safe string representation of the argument.</returns>
        public virtual string RenderSafe()
        {
            return Render();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return RenderSafe();
        }
    }
}
