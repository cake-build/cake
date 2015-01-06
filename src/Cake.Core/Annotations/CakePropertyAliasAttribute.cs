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
        /// Indicates if the result of the property alias method should be false.
        /// </summary>
        public bool Cache { get; set; }
    }
}
