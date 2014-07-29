using System;

namespace Cake.Core.Annotations
{
    /// <summary>
    /// An attribute used to mark script method aliases.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class CakeMethodAliasAttribute : CakeAliasAttribute
    {
    }
}
