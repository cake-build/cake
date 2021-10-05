// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cake.Common.Tools.DotNetCore.Format
{
    /// <summary>
    /// Contains settings used by <see cref="DotNetCoreTester" />.
    /// </summary>
    public sealed class DotNetCoreFormatSettings : DotNetCoreSettings
    {
        public ICollection<string> Diagnostics { get; set; } = new List<string>();
        public DotNetCoreSeverity? Severity { get; set; } = DotNetCoreSeverity.Warn;
        public bool NoRestore { get; set; }
        public bool VerifyNoChanges { get; set; }
        public ICollection<string> Include { get; set; } = new List<string>();
        public ICollection<string> Exclude { get; set; } = new List<string>();
        public bool IncludeGenerated { get; set; }
        public ICollection<string> BinaryLog { get; set; } = new List<string>();
        public string Report { get; set; }
    }
}
