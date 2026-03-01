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
            EmptyFiles.FileExtensions.AddTextExtension("cake");

            if (!VerifyDiffPlex.Initialized)
            {
                VerifyDiffPlex.Initialize(OutputType.Compact);
                DerivePathInfo(Expectations.Initialize);
            }

            VerifierSettings.DontScrubDateTimes();
            VerifierSettings.IgnoreMember<FakeFile>(x => x.Content);
            VerifierSettings.IgnoreMember("LastWriteTime");
            VerifierSettings.DontIgnoreEmptyCollections();
            VerifierSettings.AddExtraSettings(settings => settings.DefaultValueHandling = DefaultValueHandling.Include);
            VerifierSettings.IgnoreStackTrace();
        }
    }
}
