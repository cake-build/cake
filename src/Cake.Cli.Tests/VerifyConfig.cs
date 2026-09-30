using System.Runtime.CompilerServices;

namespace Cake.Cli.Tests;

/// <summary>
/// Configuration for Verify tests.
/// </summary>
public static class VerifyConfig
{
    /// <summary>
    /// Initializes the Verify configuration.
    /// </summary>
    [ModuleInitializer]
    public static void Init()
    {
        VerifierSettings.UseTextDiffFormat(DiffEngine.TextDiffFormat.Compact);
        DerivePathInfo(Expectations.Initialize);
    }
}