using Xunit;

namespace Cake.Testing.Xunit
{
    public sealed class WindowsTheory : TheoryAttribute
    {
        // ReSharper disable once UnusedParameter.Local
        public WindowsTheory(string reason = null)
        {
    #if __MonoCS__
            Skip = reason ?? "Windows test.";
    #endif
        }
    }
}

