// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cake.Core.Tooling;
using Cake.Testing;
using Cake.Testing.Extensions;
using Xunit;

namespace Cake.Common.Tests.CrossCutting
{
    public static class ToolSettingsTests
    {
        // Ensures that C# initializer syntax will not throw NullReferenceException when collection properties are used.
        [Theory]
        [MemberData(nameof(ToolSettingsTypes))]
        public static void Tool_settings_collection_properties_must_be_initialized(MemberTestInfo<PropertyInfo> toolSettingsProperty)
        {
            Assert.NotNull(toolSettingsProperty.Member.GetValue(toolSettingsProperty.Instance));
        }

        public static IEnumerable<object[]> ToolSettingsTypes =>
            from type in MemberTestingUtils.GetMembersToTest(typeof(ToolSettings), type =>
                type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(property => !property.PropertyType.IsArray
                                       && property.PropertyType.SatisfiesInterfaceDefinition(typeof(ICollection<>))))
            select new object[] { type };
    }
}
