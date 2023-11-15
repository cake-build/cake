using System.Runtime.CompilerServices;
using VerifyTests;
using VerifyXunit;

namespace Cake.Core.Tests
{
    public static class VerifyConfig
    {
        [ModuleInitializer]
        public static void Init()
        {
            Verifier.DerivePathInfo(Expectations.Initialize);
        }
    }
}
