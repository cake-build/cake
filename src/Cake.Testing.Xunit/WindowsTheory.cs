using Xunit;

namespace Cake.Testing.Xunit
{
    public sealed class WindowsTheory : TheoryAttribute
    {
        public WindowsTheory(string reason = null)
        {
    #if __MonoCS__
            Skip = reason ?? "Windows test.";
    #endif
        }
    }
}

