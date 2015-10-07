using Cake.Common.Tools.Chocolatey.Config;

namespace Cake.Common.Tests.Fixtures.Tools.Chocolatey
{
    public sealed class ChocolateyConfigSetterFixture : ChocolateyFixture
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public ChocolateyConfigSettings Settings { get; set; }

        public ChocolateyConfigSetterFixture()
        {
            Settings = new ChocolateyConfigSettings();
            Name = "cacheLocation";
            Value = @"c:\temp";
        }

        public void Set()
        {
            var tool = new ChocolateyConfigSetter(FileSystem, Environment, ProcessRunner, Globber, ChocolateyToolResolver);
            tool.Set(Name, Value, Settings);
        }
    }
}