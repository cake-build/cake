// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Configuration;

namespace Cake.NuGet.Tests.Stubs
{
    internal sealed class FakeNuGetSettings : ISettings
    {
        public event EventHandler SettingsChanged;

        private IDictionary<string, FakeNuGetSettingSection> _settings;

        public FakeNuGetSettings()
        {
            _settings = new Dictionary<string, FakeNuGetSettingSection>(StringComparer.OrdinalIgnoreCase);
        }

        public void AddOrUpdate(string sectionName, SettingItem item)
        {
            if (!_settings.TryGetValue(sectionName, out var section))
            {
                section = new FakeNuGetSettingSection(sectionName, null, Enumerable.Empty<SettingItem>());
                _settings[sectionName] = section;
            }

            section.AddItem(item);

            SettingsChanged?.Invoke(this, new EventArgs());
        }

        public IList<string> GetConfigFilePaths()
        {
            return Array.Empty<string>();
        }

        public IList<string> GetConfigRoots()
        {
            return Array.Empty<string>();
        }

        public SettingSection GetSection(string sectionName)
        {
            return _settings.TryGetValue(sectionName, out var value) ? value : null;
        }

        public void Remove(string sectionName, SettingItem item)
        {
        }

        public void SaveToDisk()
        {
        }
    }
}
