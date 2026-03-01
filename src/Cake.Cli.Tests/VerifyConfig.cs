using System.Runtime.CompilerServices;
using VerifyTests.DiffPlex;

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
        if (!VerifyDiffPlex.Initialized)
        {
            VerifyDiffPlex.Initialize(OutputType.Compact);
            DerivePathInfo(Expectations.Initialize);
        }
    }
}