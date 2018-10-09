using Cake.Common.NuSpec;

namespace Cake.Common.Tests.Fixtures.NuSpec
{
    internal sealed class NuSpecWithoutFileFixture : NuSpecFixtureBase
    {
        public NuSpecFixtureResult Run()
        {
            var nuSpecProcessor = new NuSpecProcessor(FileSystem, Environment, Log);
            var tmpNuSpecFile = nuSpecProcessor.Process(Environment.WorkingDirectory, Settings);
            return new NuSpecFixtureResult(FileSystem, tmpNuSpecFile);
        }
    }
}