namespace Cake.Core.Scripting
{
    /// <summary>
    /// Represents a script alias type.
    /// </summary>
    public enum ScriptAliasType
    {
        /// <summary>
        /// Represents an unknown script alias type.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Represents a script alias method.
        /// </summary>
        Method,

        /// <summary>
        /// Represents a script alias property.
        /// </summary>
        Property
    }
}