namespace Cake.MSBuild
{
    public static class MSBuildSettingsExtensions
    {
        public static MSBuildSettings WithTarget(this MSBuildSettings settings, string target)
        {
            settings.Targets.Add(target);
            return settings;
        }

        public static MSBuildSettings UseToolVersion(this MSBuildSettings settings, MSBuildToolVersion version)
        {
            settings.ToolVersion = version;
            return settings;
        }

        public static MSBuildSettings SetPlatformTarget(this MSBuildSettings settings, PlatformTarget target)
        {
            settings.PlatformTarget = target;
            return settings;
        }

        public static MSBuildSettings WithProperty(this MSBuildSettings settings, string name, string value)
        {
            settings.Properties.Add(name, value);
            return settings;
        }

        public static MSBuildSettings SetConfiguration(this MSBuildSettings settings, string configuration)
        {
            settings.Configuration = configuration;
            return settings;
        }
    }
}
