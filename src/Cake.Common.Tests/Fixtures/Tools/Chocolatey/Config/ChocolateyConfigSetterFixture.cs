using Cake.Common.Tools.Chocolatey.Config;

namespace Cake.Common.Tests.Fixtures.Tools.Chocolatey.Config
{
    internal sealed class ChocolateyConfigSetterFixture : ChocolateyFixture<ChocolateyConfigSettings>
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public ChocolateyConfigSetterFixture()
        {
            Name = "cacheLocation";
            Value = @"c:\temp";
        }

        protected override void RunTool()
        {
            var tool = new ChocolateyConfigSetter(FileSystem, Environment, ProcessRunner, Tools, Resolver);
            tool.Set(Name, Value, Settings);
        }
    }
}