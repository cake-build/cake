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
        DerivePathInfo(Expectations.Initialize);
        VerifyDiffPlex.Initialize(OutputType.Compact);
    }
}