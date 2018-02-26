namespace Cake.Core.Polyfill
{
    /// <summary>
    /// The current Runtime Cake is executing on.
    /// </summary>
    public enum Runtime
    {
        /// <summary>
        /// Full Framework or Mono.
        /// </summary>
        Clr,

        /// <summary>
        /// .NET Core 2.
        /// </summary>
        CoreClr
    }
}