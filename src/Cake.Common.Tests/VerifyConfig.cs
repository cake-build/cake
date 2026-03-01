using System.Runtime.CompilerServices;
using Argon;
using VerifyTests.DiffPlex;

namespace Cake.Common.Tests;

public static class VerifyConfig
{
    [ModuleInitializer]
    public static void Init()
    {
        EmptyFiles.FileExtensions.AddTextExtension("cake");

        if (!VerifyDiffPlex.Initialized)
        {
            VerifyDiffPlex.Initialize(OutputType.Compact);
            DerivePathInfo(Expectations.Initialize);
        }

        VerifierSettings.DontScrubDateTimes();
        VerifierSettings.DontIgnoreEmptyCollections();
        VerifierSettings.AddExtraSettings(settings => settings.DefaultValueHandling = DefaultValueHandling.Include);
        VerifierSettings.IgnoreStackTrace();
    }
}
