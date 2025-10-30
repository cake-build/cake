using System.Runtime.CompilerServices;
using Argon;
using VerifyTests.DiffPlex;

namespace Cake.Testing.Tests
{
    public static class VerifyConfig
    {
        [ModuleInitializer]
        public static void Init()
        {
            DerivePathInfo(Expectations.Initialize);
            EmptyFiles.FileExtensions.AddTextExtension("cake");
            VerifyDiffPlex.Initialize(OutputType.Compact);
            VerifierSettings.DontScrubDateTimes();
            VerifierSettings.IgnoreMember<FakeFile>(x => x.Content);
            VerifierSettings.IgnoreMember("LastWriteTime");
            VerifierSettings.DontIgnoreEmptyCollections();
            VerifierSettings.AddExtraSettings(settings => settings.DefaultValueHandling = DefaultValueHandling.Include);
            VerifierSettings.IgnoreStackTrace();
        }
    }
}
