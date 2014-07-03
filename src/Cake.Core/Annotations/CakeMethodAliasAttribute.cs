using System;

namespace Cake.Core.Annotations
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class CakeMethodAliasAttribute : Attribute
    {
    }
}
