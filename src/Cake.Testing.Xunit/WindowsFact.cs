using Xunit;

namespace Cake.Testing.Xunit
{
    public sealed class WindowsFact : FactAttribute
    {
        public WindowsFact(string reason = null)
        {
#if __MonoCS__
            Skip = reason ?? "Windows test.";
#endif
        }
    }
}