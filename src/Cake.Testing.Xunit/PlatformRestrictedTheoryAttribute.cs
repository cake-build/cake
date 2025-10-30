// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.Polyfill;
using Xunit;

namespace Cake.Testing.Xunit
{
    /// <summary>
    /// Base class for theory attributes that restrict execution to specific platforms.
    /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="PlatformRestrictedTheoryAttribute"/> class.
        /// </summary>
        /// <param name="requiredFamily">The required platform family.</param>
        /// <param name="invert">Whether to invert the platform check.</param>
        /// <param name="reason">The reason for the platform restriction.</param>
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

        /// <summary>
        /// Gets the reason for the platform restriction.
        /// </summary>
        protected string Reason { get; }

#if !XUNIT3
        /// <summary>
        /// Gets or sets the reason for skipping the test.
        /// </summary>
        public override string Skip
        {
            get => _skip ?? Reason;
            set => _skip = value;
        }
#endif
    }
}