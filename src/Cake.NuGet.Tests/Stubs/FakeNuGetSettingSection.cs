// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using NuGet.Configuration;

namespace Cake.NuGet.Tests.Stubs
{
    internal sealed class FakeNuGetSettingSection : SettingSection
    {
        public FakeNuGetSettingSection(string name, IReadOnlyDictionary<string, string> attributes, IEnumerable<SettingItem> children)
            : base(name, attributes, children)
        {
        }

        public void AddItem(SettingItem item)
        {
            Children.Add(item);
        }

        public override SettingBase Clone()
        {
            return new FakeNuGetSettingSection(ElementName, MutableAttributes, Items);
        }
    }
}
