using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using VerifyTests;
using static VerifyXunit.Verifier;

namespace Cake.Core.Tests;

public static class VerifyConfig
{
    private static bool _initialized;

    [ModuleInitializer]
    public static void Init()
    {
        // The runtime invokes this module initializer, and Cake.Common.Tests calls
        // it again so that assembly is loaded before any Verify run.
        if (_initialized)
        {
            return;
        }

        _initialized = true;

        EmptyFiles.FileExtensions.AddTextExtension(Extensions.Cake);
        VerifierSettings.UseTextDiffFormat(DiffEngine.TextDiffFormat.Compact);
        DerivePathInfo(Expectations.Initialize);
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
