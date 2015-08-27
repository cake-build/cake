using System;
using System.Linq;
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
          var directive = tokens.FirstOrDefault();
          if (!string.IsNullOrWhiteSpace(directive))
          {
            if (directive.Equals("#addin", StringComparison.OrdinalIgnoreCase))
            {
              // Fetch the addin NuGet ID.
              arguments.AddInId = tokens
                .Select(value => value.UnQuote())
                .Skip(1).FirstOrDefault();

              if (!string.IsNullOrWhiteSpace(arguments.AddInId))
              {
                // Fetch optional NuGet source.
                arguments.Source = tokens
                  .Skip(2)
                  .Select(value => value.UnQuote())
                  .FirstOrDefault();

                // Get the directory path to Cake.
                var applicationRoot = _environment.GetApplicationRoot();

                // Get the addin directory.
                var addInRootDirectoryPath = applicationRoot
                  .Combine("..\\Addins")
                  .Collapse()
                  .MakeAbsolute(_environment);

                arguments.AddInDirectoryPath = addInRootDirectoryPath.Combine(arguments.AddInId);
                arguments.AddInRootDirectory = _fileSystem.GetDirectory(addInRootDirectoryPath);
                arguments.Valid = true;
              }
            }
          }
        }
      }
      catch (Exception e)
      {
        // Log this error.
        const string format = "An error occured while parsing line {0}.";
        _log.Error(format, string.Join(" ", tokens));
        _log.Error("Error: {0}", _log.Verbosity == Verbosity.Diagnostic ? e.ToString() : e.Message);
        arguments.Valid = false;
      }

      return arguments;
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
      /// Gets or sets a the add-in source (Nuget.org used if missing)
      /// </summary>
      public string Source { get; set; }

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
    }
  }


}
