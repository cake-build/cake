using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Core.Scripting.Processors.Parsers
{
  /// <summary>
  /// Parses the information from an #addin line
  /// </summary>
  public class AddInDirectiveParser
  {
    private readonly ICakeEnvironment _environment;
    private readonly IFileSystem _fileSystem;
    private readonly ICakeLog _log;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddInDirectiveParser" /> class.
    /// </summary>
    /// <param name="fileSystem">The file system.</param>
    /// <param name="environment">The environment.</param>
    /// <param name="log">The logger</param>
    public AddInDirectiveParser(ICakeEnvironment environment, IFileSystem fileSystem, ICakeLog log)
    {
      _environment = environment;
      _fileSystem = fileSystem;
      _log = log;
    }

    /// <summary>
    /// Parses the information from an #addin line
    /// </summary>
    /// <param name="tokens">The #addin line split by space (' ')</param>
    /// <returns>Populated AddInDirectiveArguments</returns>
    public AddInDirectiveArguments Parse(string[] tokens)
    {
      var arguments = new AddInDirectiveArguments();
      try
      {
        if (tokens != null && tokens.Any())
        {
          List<string> tokenList = tokens.ToList();
          if (tokenList.Count > 1)
          {
            var directive = tokenList[0];
            if (directive.Equals("#addin", StringComparison.OrdinalIgnoreCase))
            {
              // Fetch the addin NuGet ID.
              arguments.AddInId = tokenList[1].UnQuote();
              // Get the directory path to Cake.
              var applicationRoot = _environment.GetApplicationRoot();

              // Get the addin directory.
              var addInRootDirectoryPath = applicationRoot
                .Combine("..\\Addins")
                .Collapse()
                .MakeAbsolute(_environment);

              arguments.AddInDirectoryPath = addInRootDirectoryPath.Combine(arguments.AddInId);
              arguments.AddInRootDirectory = _fileSystem.GetDirectory(addInRootDirectoryPath);

              var argumentBuilder = new ProcessArgumentBuilder();
              argumentBuilder.Append("install");
              argumentBuilder.AppendQuoted(arguments.AddInId);

              ExtractVersionFolder(tokenList, arguments);

              if (tokenList.Count > 2)
              {
                // if there are any parameters starting with a hyphen then just pass the lot to NuGet
                if (tokenList[2].StartsWith("-"))
                {
                  // We will add an output folder - ignore any handed in
                  RemoveToken(tokenList, "-o", true);
                  if (tokenList.Count > 2)
                  {
                    argumentBuilder.Append(string.Join(" ", tokenList.Skip(2)).Trim());
                  }
                }
                else
                {
                  // No hyphens - must be just the optional NuGet source
                  argumentBuilder.Append("-Source");
                  argumentBuilder.AppendQuoted(tokenList[2]);
                }
              }

              // Add compulsory 
              AddTokenIfMissing(argumentBuilder, tokenList, "-ExcludeVersion");
              AddTokenIfMissing(argumentBuilder, tokenList, "-NonInteractive");
              AddTokenIfMissing(argumentBuilder, tokenList, "-NoCache");
              argumentBuilder.Append("-OutputDirectory");
              argumentBuilder.AppendQuoted(arguments.AddInRootDirectory.Path.FullPath);

              arguments.InstallArguments = argumentBuilder;
              arguments.Valid = true;
            }
          }
        }
      }
      catch (Exception e)
      {
        const string format = "An error occured while parsing line {0}.";

        // ReSharper disable once AssignNullToNotNullAttribute
        _log.Error(format, string.Join(" ", tokens));
        _log.Error("Error: {0}", _log.Verbosity == Verbosity.Diagnostic ? e.ToString() : e.Message);
        arguments.Valid = false;
      }

      return arguments;
    }

    private void ExtractVersionFolder(List<string> tokenList, AddInDirectiveArguments arguments)
    {
      string found =
        tokenList.FirstOrDefault(t => String.Compare(t, "-VersionFolder", StringComparison.InvariantCultureIgnoreCase) == 0);
      if (found == null)
      {
        arguments.VersionFolder = GetCurrentNetVersion();
      }
      else
      {
        arguments.VersionFolder = tokenList[tokenList.IndexOf(found) + 1];
        RemoveToken(tokenList, "-VersionFolder", true);
      }
    }
    private const string NetFrameworkIdentifier = ".NETFramework";
    private const string AspNetFrameworkIdentifier = "ASP.NET";
    private const string AspNetCoreFrameworkIdentifier = "ASP.NETCore";
    private static readonly Dictionary<string, string> _identifierToFrameworkFolder = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) {
            { NetFrameworkIdentifier, "net" },
            { ".NETMicroFramework", "netmf" },
            { AspNetFrameworkIdentifier, "aspnet" },
            { AspNetCoreFrameworkIdentifier, "aspnetcore" },
            { "Silverlight", "sl" },
            { ".NETCore", "win"},
            { "Windows", "win"},
            { ".NETPortable", "portable" },
            { "WindowsPhone", "wp"},
            { "WindowsPhoneApp", "wpa"},
            { "Xamarin.iOS", "xamarinios" },
            { "Xamarin.Mac", "xamarinmac" },
            { "Xamarin.PlayStation3", "xamarinpsthree" },
            { "Xamarin.PlayStation4", "xamarinpsfour" },
            { "Xamarin.PlayStationVita", "xamarinpsvita" },
            { "Xamarin.Xbox360", "xamarinxboxthreesixty" },
            { "Xamarin.XboxOne", "xamarinxboxone" }
        };
    private string GetCurrentNetVersion()
    {
      var frameworkName = new FrameworkName(AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName);

      string name;
      if (!_identifierToFrameworkFolder.TryGetValue(frameworkName.Identifier, out name))
      {
        name = frameworkName.Identifier;
      }

      // only show version part if it's > 0.0.0.0
      if (frameworkName.Version > new Version())
      {
        // Remove the . from versions
        name += frameworkName.Version.ToString().Replace(".", String.Empty);
      }

      return name;
      // Code taken from https://github.com/NuGet/NuGet2/blob/861741bd780225a542c73a1d0904dd33b33a7a3d/src/Core/Utility/VersionUtility.cs
      ////// Do a reverse lookup in _frameworkNameAlias. This is so that we can produce the more user-friendly
      ////// "windowsphone" string, rather than "sl3-wp". The latter one is also prohibited in portable framework's profile string.
      ////foreach (KeyValuePair<FrameworkName, FrameworkName> pair in _frameworkNameAlias)
      ////{
      ////  // use our custom equality comparer because we want to perform case-insensitive comparison
      ////  if (FrameworkNameEqualityComparer.Default.Equals(pair.Value, frameworkName))
      ////  {
      ////    frameworkName = pair.Key;
      ////    break;
      ////  }
      ////}

      ////string name;
      ////if (!_identifierToFrameworkFolder.TryGetValue(frameworkName.Identifier, out name))
      ////{
      ////name = frameworkName.Identifier;
      ////}

      ////// for Portable framework name, the short name has the form "portable-sl4+wp7+net45"
      ////string profile;
      ////if (name.Equals("portable", StringComparison.OrdinalIgnoreCase))
      ////{
        ////if (portableProfileTable == null)
        ////{
        ////  throw new ArgumentException(NuGetResources.PortableProfileTableMustBeSpecified, "portableProfileTable");
        ////}
        ////NetPortableProfile portableProfile = NetPortableProfile.Parse(frameworkName.Profile, portableProfileTable: portableProfileTable);
        ////if (portableProfile != null)
        ////{
        ////  profile = portableProfile.CustomProfileString;
        ////}
        ////else
        ////{
          ////profile = frameworkName.Profile;
        ////}
      ////}
      ////else
      ////{
        // only show version part if it's > 0.0.0.0
        ////if (frameworkName.Version > new Version())
        ////{
          ////// Remove the . from versions
        ////  name += frameworkName.Version.ToString().Replace(".", String.Empty);
        ////}

        ////if (String.IsNullOrEmpty(frameworkName.Profile))
        ////{
        ////  return name;
        ////}

        ////if (!_identifierToProfileFolder.TryGetValue(frameworkName.Profile, out profile))
        ////{
        ////  profile = frameworkName.Profile;
        ////}
      ////}

      ////return name + "-" + profile;
    }

    private void RemoveToken(List<string> tokenList, string token, bool removeNextValue)
    {
      string found =
        tokenList.FirstOrDefault(t => t.ToLowerInvariant().StartsWith(token.ToLowerInvariant()));
      if (found != null)
      {
        if (removeNextValue)
        {
          int index = tokenList.IndexOf(found);
          tokenList.Remove(found);
          tokenList.RemoveAt(index);
        }
        else
        {
          tokenList.Remove(found);
        }
      }
    }

    private void AddTokenIfMissing(ProcessArgumentBuilder builder, List<string> tokenList, string token)
    {
      if (tokenList.All(t => String.Compare(t, token, StringComparison.InvariantCultureIgnoreCase) != 0))
      {
        builder.Append(token);
      }
    }

    /// <summary>
    /// Add In Directive Arguments
    /// </summary>
    public class AddInDirectiveArguments
    {
      /// <summary>
      /// Gets or sets a the identifier for this add-in
      /// </summary>
      public string AddInId { get; set; }

      /// <summary>
      /// Gets or sets a path to this add-in's install path
      /// </summary>
      public DirectoryPath AddInDirectoryPath { get; set; }

      /// <summary>
      /// Gets or sets a path to parent path to all add-in installs
      /// </summary>
      public IDirectory AddInRootDirectory { get; set; }

      /// <summary>
      /// Gets or sets a value indicating whether the line was successfully parsed as an add-in
      /// </summary>
      public bool Valid { get; set; }

      /// <summary>
      /// Gets or sets a complete set of arguments to pass to NuGet
      /// </summary>
      public ProcessArgumentBuilder InstallArguments { get; set; }


      /// <summary>
      /// Gets or sets a .Net version e.g. "net45"
      /// </summary>
      public string VersionFolder { get; set; }
    }
  }


}
