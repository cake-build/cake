using System.Runtime.CompilerServices;
using Argon;

namespace Cake.Testing.Tests;

public static class VerifyConfig
{
    [ModuleInitializer]
    public static void Init()
    {
        EmptyFiles.FileExtensions.AddTextExtension("cake");

        VerifierSettings.UseTextDiffFormat(DiffEngine.TextDiffFormat.Compact);
        DerivePathInfo(Expectations.Initialize);

        VerifierSettings.DontScrubDateTimes();
        VerifierSettings.IgnoreMember<FakeFile>(x => x.Content);
        VerifierSettings.IgnoreMember("LastWriteTime");
        VerifierSettings.DontIgnoreEmptyCollections();
        VerifierSettings.AddExtraSettings(settings => settings.DefaultValueHandling = DefaultValueHandling.Include);
        VerifierSettings.IgnoreStackTrace();
    }
}
