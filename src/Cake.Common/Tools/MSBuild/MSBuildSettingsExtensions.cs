using System;
using System.Collections.Generic;
using System.Linq;

namespace Cake.Common.Tools.MSBuild
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

        public static MSBuildSettings WithProperty(this MSBuildSettings settings, string name, params string[] values)
        {
            IList<string> currValue;
            currValue = new List<string>(
                settings.Properties.TryGetValue(name, out currValue) && currValue != null
                    ? currValue.Concat(values)
                    : values
                );
            
            settings.Properties[name] = currValue;
            return settings;
        }

        public static MSBuildSettings SetConfiguration(this MSBuildSettings settings, string configuration)
        {
            settings.Configuration = configuration;
            return settings;
        }

        public static MSBuildSettings SetMaxCpuCount(this MSBuildSettings settings, int maxCpuCount)
        {
            settings.MaxCpuCount = Math.Max(0, maxCpuCount);
            return settings;
        }
    }
}
