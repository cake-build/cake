namespace Cake.Core.Text
{
    /// <summary>
    /// Represents a text template.
    /// </summary>
    public interface ITextTransformationTemplate
    {
        /// <summary>
        /// Registers a key and an associated value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        void Register(string key, object value);

        /// <summary>
        /// Renders the text template using the registered tokens.
        /// </summary>
        /// <returns>The rendered template.</returns>
        string Render();
    }
}
