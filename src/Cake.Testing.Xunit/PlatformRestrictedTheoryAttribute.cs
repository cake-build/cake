// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.Polyfill;
using Xunit;

namespace Cake.Testing.Xunit
{
    public abstract class PlatformRestrictedTheoryAttribute : TheoryAttribute
    {
        private static readonly PlatformFamily _family;
#if !XUNIT3
        private string _skip;
#endif

        static PlatformRestrictedTheoryAttribute()
        {
            _family = EnvironmentHelper.GetPlatformFamily();
        }

        protected PlatformRestrictedTheoryAttribute(
            PlatformFamily requiredFamily,
            bool invert,
            string reason = null)
        {
            if ((requiredFamily != _family) ^ invert)
            {
                if (string.IsNullOrEmpty(reason))
                {
                    var platformName = Enum.GetName(requiredFamily);
                    if (invert)
                    {
                        platformName = $"Non-{platformName}";
                    }

                    reason = $"{platformName} test.";
                }

                Reason = reason;
#if XUNIT3
                if (!string.IsNullOrEmpty(reason) && string.IsNullOrEmpty(Skip))
                {
                    Skip = reason;
                }
#endif
            }
        }

        protected string Reason { get; }

#if !XUNIT3
        public override string Skip
        {
            get => _skip ?? Reason;
            set => _skip = value;
        }
#endif
    }
}