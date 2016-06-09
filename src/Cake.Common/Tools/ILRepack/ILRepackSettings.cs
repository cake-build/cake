// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using Cake.Common.Tools.ILMerge;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.ILRepack
{
    /// <summary>
    /// Contains settings used by <see cref="ILRepackRunner"/>.
    /// </summary>
    public sealed class ILRepackSettings : ToolSettings
    {
        /*
        Syntax: ILRepack.exe [options] /out:<path> <path_to_primary> [<other_assemblies> ...]

        - /help              displays this usage
        - /keyfile:<path>    specifies a keyfile to sign the output assembly
        - /log:<logfile>     enable logging (to a file, if given) (default is disabled)
        - /ver:M.X.Y.Z       target assembly version
        - /union             merges types with identical names into one
        - /ndebug            disables symbol file generation
        - /copyattrs         copy assembly attributes (by default only the primary assembly attributes are copied)
        - /attr:<path>       take assembly attributes from the given assembly file
        - /allowMultiple     when copyattrs is specified, allows multiple attributes (if type allows)
        - /target:kind       specify target assembly kind (library, exe, winexe supported, default is same as first assembly)
        - /targetplatform:P  specify target platform (v1, v1.1, v2, v4 supported)
        - /xmldocs           merges XML documentation as well
        - /lib:<path>        adds the path to the search directories for referenced assemblies (can be specified multiple times)
        - /internalize       sets all types but the ones from the first assembly 'internal'
        - /delaysign         sets the key, but don't sign the assembly
        - /usefullpublickeyforreferences - NOT IMPLEMENTED
        - /align             - NOT IMPLEMENTED
        - /closed            - NOT IMPLEMENTED
        - /allowdup:Type     allows the specified type for being duplicated in input assemblies
        - /allowduplicateresources allows to duplicate resources in output assembly (by default they're ignored)
        - /zeropekind        allows assemblies with Zero PeKind (but obviously only IL will get merged)
        - /wildcards         allows (and resolves) file wildcards (e.g. `*`.dll) in input assemblies
        - /parallel          use as many CPUs as possible to merge the assemblies
        - /pause             pause execution once completed (good for debugging)
        - /verbose           shows more logs
        - /out:<path>        target assembly path, symbol/config/doc files will be written here as well
        - <path_to_primary>  primary assembly, gives the name, version to the merged one
        - <other_assemblies> ...
        */

        /// <summary>
        /// Gets or sets a keyfile to sign the output assembly
        /// </summary>
        /// <value>The keyfile.</value>
        public FilePath Keyfile { get; set; }

        /// <summary>
        /// Gets or sets a file to enable logging to (no logging if null or empty)
        /// </summary>
        /// <value>The log file.</value>
        public string Log { get; set; }

        /// <summary>
        /// Gets or sets the target assembly version.
        /// </summary>
        /// <value>The version.</value>
        public Version Version { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to merge types with identical names into one
        /// </summary>
        /// <value><c>true</c> if types with identical names should be merged into one; otherwise, <c>false</c>.</value>
        public bool Union { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to disable symbol file generation
        /// </summary>
        /// <value><c>true</c> if debug symbols should be disabled; otherwise, <c>false</c>.</value>
        public bool NDebug { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to copy assembly attributes (by default only the primary assembly attributes are copied)
        /// </summary>
        /// <value><c>true</c> if assembly attributes should be copied; otherwise, <c>false</c>.</value>
        public bool CopyAttrs { get; set; }

        /// <summary>
        /// Gets or sets the assembly file to take attributes from
        /// </summary>
        /// <value>The assembly file to take attributes from.</value>
        public FilePath Attr { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow multiple attributes (if type allows)
        /// </summary>
        /// <value><c>true</c> if multiple attributes should be allowed; otherwise, <c>false</c>.</value>
        public bool AllowMultiple { get; set; }

        /// <summary>
        /// Gets or sets the specify target assembly kind (library, exe, winexe supported, default is same as first assembly)
        /// </summary>
        /// <value>The kind of the target assembly to create.</value>
        public TargetKind TargetKind { get; set; }

        /// <summary>
        /// Gets or sets the target platform (v1, v1.1, v2, v4 supported)
        /// </summary>
        /// <value>The target platform.</value>
        public TargetPlatformVersion? TargetPlatform { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to merge XML documentation as well
        /// </summary>
        /// <value><c>true</c> if xml documents should be merged; otherwise, <c>false</c>.</value>
        public bool XmlDocs { get; set; }

        /// <summary>
        /// Gets or sets the paths to search directories for referenced assemblies (can be specified multiple times)
        /// </summary>
        /// <value>The libs.</value>
        public List<FilePath> Libs { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to set all types but the ones from the first assembly 'internal'
        /// </summary>
        /// <value>
        /// <c>true</c> if types in assemblies other than the primary assembly should
        /// have their visibility modified to internal; otherwise, <c>false</c>.
        /// </value>
        public bool Internalize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to set the key, but don't sign the assembly
        /// </summary>
        /// <value><c>true</c> if assembly should be delay signed; otherwise, <c>false</c>.</value>
        public bool DelaySign { get; set; }

        /// <summary>
        /// Gets or sets the specified type for being duplicated in input assemblies
        /// </summary>
        /// <value>The type to allow duplication of.</value>
        public string AllowDup { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to duplicate resources in output assembly (by default they're ignored)
        /// </summary>
        /// <value><c>true</c> if duplicate resources should be allowed in the output assembly; otherwise, <c>false</c>.</value>
        public bool AllowDuplicateResources { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow assemblies with Zero PeKind (but obviously only IL will get merged)
        /// </summary>
        /// <value><c>true</c> if assemblies with Zero PeKind should be allowed; otherwise, <c>false</c>.</value>
        public bool ZeroPeKind { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow (and resolve) file wildcards (e.g. `*`.dll) in input assemblies
        /// </summary>
        /// <value><c>true</c> if file wildcards should be allowed in input assembly paths; otherwise, <c>false</c>.</value>
        public bool Wildcards { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use as many CPUs as possible to merge the assemblies
        /// </summary>
        /// <value><c>true</c> if merging should use as many CPUs as possible in parallel; otherwise, <c>false</c>.</value>
        public bool Parallel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to pause execution once completed (good for debugging)
        /// </summary>
        /// <value><c>true</c> if execution should pause once completed; otherwise, <c>false</c>.</value>
        public bool Pause { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show more logs
        /// </summary>
        /// <value><c>true</c> if more logs should be output during execution; otherwise, <c>false</c>.</value>
        public bool Verbose { get; set; }
    }
}
