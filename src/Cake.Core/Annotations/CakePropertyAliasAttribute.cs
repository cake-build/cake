using System;

namespace Cake.Core.Annotations
{
    /// <summary>
    /// An attribute used to mark script property aliases.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class CakePropertyAliasAttribute : CakeAliasAttribute
    {
        /// <summary>
        /// Gets or sets a value indicating whether the result of the property alias method should be cached.
        /// Indicates .
        /// </summary>
        /// <value>
        ///   <c>true</c> if cache; otherwise, <c>false</c>.
        /// </value>
        public bool Cache { get; set; }
    }
}