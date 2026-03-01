using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using VerifyTests;
using VerifyTests.DiffPlex;
using static VerifyXunit.Verifier;

namespace Cake.Core.Tests
{
    public static class VerifyConfig
    {
        [ModuleInitializer]
        public static void Init()
        {
            EmptyFiles.FileExtensions.AddTextExtension(Extensions.Cake);

            if (!VerifyDiffPlex.Initialized)
            {
                VerifyDiffPlex.Initialize(OutputType.Compact);
                DerivePathInfo(Expectations.Initialize);
            }
        }

        public static class Extensions
        {
            public const string Cake = "cake";
        }

        [Pure]
        public static SettingsTask VerifyCake(
                string target,
                VerifySettings settings = null,
                [CallerFilePath] string sourceFile = "")
            => Verify(target, Extensions.Cake, settings, sourceFile);
    }
}
