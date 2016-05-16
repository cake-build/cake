namespace Cake.Core.Configuration
{
    /// <summary>
    /// Represents the Cake configuration.
    /// </summary>
    public interface ICakeConfiguration
    {
        /// <summary>
        /// Gets the value that corresponds to the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value for the specified key, or <c>null</c> if key doesn't exists.</returns>
        string GetValue(string key);
    }
}
