// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.ILMerge
{
    /// <summary>
    /// Contains settings used by <see cref="ILMergeRunner"/>.
    /// </summary>
    public sealed class ILMergeSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether whether types in assemblies other
        /// than the primary assembly should have their visibility modified to internal.
        /// </summary>
        /// <value>
        /// <c>true</c> if types in assemblies other than the primary assembly should
        /// have their visibility modified to internal; otherwise, <c>false</c>.
        /// </value>
        public bool Internalize { get; set; }

        /// <summary>
        /// Gets or sets the target kind.
        /// </summary>
        /// <value>The target kind.</value>
        public TargetKind TargetKind { get; set; }

        /// <summary>
        /// Gets or sets the target platform.
        /// </summary>
        /// <value>The target platform.</value>
        public TargetPlatform TargetPlatform { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the target assembly will be
        /// delay signed.
        /// </summary>
        /// <value>
        /// <c>true</c> if target assembly will be delay signed; otherwise, <c>false</c>.
        /// </value>
        /// <remark>This can be set only in conjunction with the <see cref="KeyFile"/> option.</remark>
        public bool DelaySign { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the "transitive closure" of the
        /// input assemblies is computed and added to the list of input assemblies.
        /// </summary>
        /// <value>
        /// <c>true</c> if the "transitive closure" of the input assemblies is computed
        /// and added to the list of input assemblies; otherwise, <c>false</c>.
        /// </value>
        /// <remark>
        /// An assembly is considered part of the transitive closure if it is
        /// referenced, either directly or indirectly, from one of the originally
        /// specified input assemblies and it has an external reference to one of
        /// the input assemblies, or one of the assemblies that has such a reference.
        /// </remark>
        public bool Closed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a .pdb file for the output assembly
        /// is generated and merges into it any .pdb files found for input assemblies.
        /// </summary>
        /// <value>
        /// <c>true</c> if pdb file is generated for output assembly; otherwise, <c>false</c>.
        /// </value>
        public bool NDebug { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the assembly level attributes of
        /// each input assembly are copied over into the target assembly.
        /// </summary>
        /// <value>
        /// <c>true</c> if the assembly level attributes are copied to target
        /// assembly; otherwise, <c>false</c>.
        /// </value>
        public bool CopyAttributes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether you want to allow duplicates (for
        /// those attributes whose type specifies "AllowMultiple" in their definition).
        /// </summary>
        /// <value>
        /// <c>true</c> if duplicates are allowed; otherwise, <c>false</c>.
        /// </value>
        /// <remark>This can be set only in conjunction with the <see cref="CopyAttributes"/> option.</remark>
        public bool AllowMultiple { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the first attribute that is found is kept.
        /// </summary>
        /// <value>
        /// <c>true</c> if the first attribute that is found is kept; otherwise, <c>false</c>.
        /// </value>
        /// <remark>This can be set only in conjunction with the <see cref="CopyAttributes"/> option.</remark>
        public bool KeepFirst { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether XML documentation files are merged
        /// to produce an XML documentation file for the target assembly.
        /// </summary>
        /// <value>
        /// <c>true</c> if XML documentation files are merged; otherwise, <c>false</c>.
        /// </value>
        public bool XmlDocumentation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether external assembly references
        /// in the manifest of the target assembly will use full public keys or
        /// public key tokens.
        /// </summary>
        /// <value>
        /// <c>true</c> when full public keys should be used; otherwise, <c>false</c>.
        /// </value>
        public bool UseFullPublicKeyForReferences { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether any wild cards in file names
        /// are expanded and all matching files will be used as input.
        /// </summary>
        /// <value>
        /// <c>true</c> if wildcards in file names are expanded; otherwise, <c>false</c>.
        /// </value>
        public bool Wildcards { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether an assembly's PeKind flag (this
        /// is the value of the field listed as .corflags in the Manifest) is zero
        /// it will be treated as if it was ILonly.
        /// </summary>
        /// <value>
        /// <c>true</c> when assembly's PeKind flag is zero; otherwise, <c>false</c>.
        /// </value>
        public bool ZeroPeKind { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether types with the same name are all
        /// merged into a single type in the target assembly.
        /// </summary>
        /// <value>
        /// <c>true</c> if types with the same name are merged into a single type in
        /// the target assembly; otherwise, <c>false</c>.
        /// </value>
        /// <remark>Cannot be specified at the same time as <see cref="AllowDuplicateTypes"/>.</remark>
        public bool Union { get; set; }

        /// <summary>
        /// Gets or sets a value that controls the file alignment used for the target assembly.
        /// </summary>
        public int? Align { get; set; }

        /// <summary>
        /// Gets or sets the path and filename to an attribute assembly, an assembly that
        /// will be used to get all of the assembly-level attributes such as Culture,
        /// Version, etc.
        /// </summary>
        public FilePath AttributeFile { get; set; }

        /// <summary>
        /// Gets or sets the version. When this has a non-null value, then the target assembly will be given its
        /// value as the version number of the assembly.
        /// </summary>
        /// <remark>
        /// The version must be a valid assembly version as defined by the attribute
        /// AssemblyVersion in the System.Reflection namespace.
        /// </remark>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether log messages are written.
        /// </summary>
        /// <remark>
        /// If Log is true, but <see cref="LogFile"/> is null, then log messages are written to
        /// Console.Out.
        /// </remark>
        public bool Log { get; set; }

        /// <summary>
        /// Gets or sets the path to the file where log messages should be written to.
        /// </summary>
        public FilePath LogFile { get; set; }

        /// <summary>
        /// Gets or sets the path to a .snk file. The target assembly will be signed with
        /// its contents and will then have a strong name.
        /// </summary>
        public FilePath KeyFile { get; set; }

        /// <summary>
        /// Gets or sets the name of the container to use when signing the target assembly.
        /// </summary>
        public string KeyContainer { get; set; }

        /// <summary>
        /// Gets or sets the directories to be used to search for input assemblies.
        /// </summary>
        public DirectoryPath[] SearchDirectories { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow the user to allow all
        /// public types to be renamed when they are duplicates.
        /// </summary>
        /// <value>
        /// <c>true</c> if all public types should be allowed to be renamed; otherwise <c>false</c>.
        /// </value>
        /// <remark>
        /// Use <see cref="DuplicateTypes"/> to allow fine grain control over exactly
        /// which types are allowed to be renamed.
        /// </remark>
        public bool AllowDuplicateTypes { get; set; }

        /// <summary>
        /// Gets or sets a list of public types which are allowed to be renamed when duplicates
        /// exist.
        /// </summary>
        public string[] DuplicateTypes { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ILMergeSettings"/> class.
        /// </summary>
        public ILMergeSettings()
        {
            Internalize = false;
            TargetKind = TargetKind.Default;
        }
    }
}