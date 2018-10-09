using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.NuSpec
{
    /// <summary>
    /// Contains functionality for working with NuSpec files.
    /// </summary>
    public static class NuSpecAliases
    {
        /// <summary>
        /// Creates a NuGet package using the specified Nuspec or project file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePath">The nuspec or project file path.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        ///     var nuSpecSettings   = new NuSpecSettings {
        ///                                Id                       = "TestNuget",
        ///                                Version                  = "0.0.0.1",
        ///                                Title                    = "The tile of the package",
        ///                                Authors                  = new[] {"John Doe"},
        ///                                Owners                   = new[] {"Contoso"},
        ///                                Description              = "The description of the package",
        ///                                Summary                  = "Excellent summary of what the package does",
        ///                                ProjectUrl               = new Uri("https://github.com/SomeUser/TestNuget/"),
        ///                                IconUrl                  = new Uri("http://cdn.rawgit.com/SomeUser/TestNuget/master/icons/testnuget.png"),
        ///                                LicenseUrl               = new Uri("https://github.com/SomeUser/TestNuget/blob/master/LICENSE.md"),
        ///                                Copyright                = "Some company 2015",
        ///                                ReleaseNotes             = new [] {"Bug fixes", "Issue fixes", "Typos"},
        ///                                Tags                     = new [] {"Cake", "Script", "Build"},
        ///                                RequireLicenseAcceptance = false,
        ///                                Files                    = new [] { new NuSpecContent {Source = "bin/TestNuget.dll", Target = "bin"}, },
        ///                          };
        ///
        ///     NuSpec("./nuspec/TestNuget.nuspec", nuGetPackSettings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("NuSpec")]
        [CakeNamespaceImport("Cake.Common.NuSpec")]
        public static void NuSpec(this ICakeContext context, FilePath filePath, NuSpecSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var nuSpecProcessor = new NuSpecProcessor(context.FileSystem, context.Environment, context.Log);
            nuSpecProcessor.Process(filePath, settings);
        }

        /// <summary>
        /// Creates a NuGet package using the specified Nuspec or project file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        ///     var nuSpecSettings   = new NuSpecSettings {
        ///                                Id                       = "TestNuget",
        ///                                Version                  = "0.0.0.1",
        ///                                Title                    = "The tile of the package",
        ///                                Authors                  = new[] {"John Doe"},
        ///                                Owners                   = new[] {"Contoso"},
        ///                                Description              = "The description of the package",
        ///                                Summary                  = "Excellent summary of what the package does",
        ///                                ProjectUrl               = new Uri("https://github.com/SomeUser/TestNuget/"),
        ///                                IconUrl                  = new Uri("http://cdn.rawgit.com/SomeUser/TestNuget/master/icons/testnuget.png"),
        ///                                LicenseUrl               = new Uri("https://github.com/SomeUser/TestNuget/blob/master/LICENSE.md"),
        ///                                Copyright                = "Some company 2015",
        ///                                ReleaseNotes             = new [] {"Bug fixes", "Issue fixes", "Typos"},
        ///                                Tags                     = new [] {"Cake", "Script", "Build"},
        ///                                RequireLicenseAcceptance = false,
        ///                                Files                    = new [] { new NuSpecContent {Source = "bin/TestNuget.dll", Target = "bin"}, },
        ///                          };
        ///
        ///     NuSpec(nuGetPackSettings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("NuSpec")]
        [CakeNamespaceImport("Cake.Common.NuSpec")]
        public static void NuSpec(this ICakeContext context, NuSpecSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var nuSpecProcessor = new NuSpecProcessor(context.FileSystem, context.Environment, context.Log);
            nuSpecProcessor.Process(context.Environment.WorkingDirectory, settings);
        }
    }
}
