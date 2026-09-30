using System.Runtime.CompilerServices;
using Argon;

namespace Cake.Common.Tests;

public static class VerifyConfig
{
    [ModuleInitializer]
    public static void Init()
    {
        // Load Cake.Core.Tests now. On net11 its module initializer otherwise
        // runs on first use, which can be after Verify has already started.
        Cake.Core.Tests.VerifyConfig.Init();

        EmptyFiles.FileExtensions.AddTextExtension("cake");

        VerifierSettings.DontScrubDateTimes();
        VerifierSettings.DontIgnoreEmptyCollections();
        VerifierSettings.AddExtraSettings(settings => settings.DefaultValueHandling = DefaultValueHandling.Include);
        VerifierSettings.IgnoreStackTrace();
    }
}
