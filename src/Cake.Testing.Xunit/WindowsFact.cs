using Xunit;

namespace Cake.Testing.Xunit
{
    public sealed class WindowsFact : FactAttribute
    {
        // ReSharper disable once UnusedParameter.Local
        public WindowsFact(string reason = null)
        {
#if __MonoCS__
            Skip = reason ?? "Windows test.";
#endif
        }
    }
}