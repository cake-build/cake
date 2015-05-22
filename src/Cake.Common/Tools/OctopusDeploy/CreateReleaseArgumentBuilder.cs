using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.OctopusDeploy
{
    internal class CreateReleaseArgumentBuilder
    {
        private readonly string _projectName;
        private readonly CreateReleaseSettings _settings;
        private readonly ICakeEnvironment _environment;

        private readonly ProcessArgumentBuilder _builder;

        public CreateReleaseArgumentBuilder(string projectName, CreateReleaseSettings settings, ICakeEnvironment environment)
        {
            _projectName = projectName;
            _settings = settings;
            _environment = environment;
            _builder = new ProcessArgumentBuilder("--{0} {1}");
        }

        public ProcessArgumentBuilder Get()
        {
            AppendCommonArguments();

            AppendArgumentIfNotNull("releaseNumber", _settings.ReleaseNumber);
            AppendArgumentIfNotNull("defaultpackageversion", _settings.DefaultPackageVersion);
            AppendPackages(_settings, _builder);
            AppendArgumentIfNotNull("packagesFolder", _settings.PackagesFolder);
            AppendArgumentIfNotNull("releasenotes", _settings.ReleaseNotes);
            AppendArgumentIfNotNull("releasenotesfile", _settings.ReleaseNotesFile);

            if (_settings.IgnoreExisting)
            {
                _builder.Append("--ignoreexisting");
            }

            return _builder;
        }

        private void AppendCommonArguments()
        {
            _builder.Append("create-release")
                .AppendNamedQuoted("project", _projectName)
                .AppendNamed("server", _settings.Server)
                .AppendNamedSecret("apiKey", _settings.ApiKey);

            AppendArgumentIfNotNull("username", _settings.Username);

            if (_settings.Password != null)
            {
                _builder.AppendNamedQuotedSecret("password", _settings.Password);
            }

            AppendArgumentIfNotNull("configFile", _settings.ConfigurationFile);

            if (_settings.EnableDebugLogging)
            {
                _builder.Append("--debug");
            }

            if (_settings.IgnoreSslErrors)
            {
                _builder.Append("--ignoreSslErrors");
            }

            if (_settings.EnableServiceMessages)
            {
                _builder.Append("--enableServiceMessages");
            }
        }

        private void AppendArgumentIfNotNull(string argumentName, string value)
        {
            if (value != null)
            {
                _builder.AppendNamedQuoted(argumentName, value);
            }
        }

        private void AppendArgumentIfNotNull(string argumentName, FilePath value)
        {
            if (value != null)
            {
                _builder.AppendNamedQuoted(argumentName, value.MakeAbsolute(_environment).FullPath);
            }
        }

        private static void AppendPackages(CreateReleaseSettings settings, ProcessArgumentBuilder builder)
        {
            if (settings.Packages != null)
            {
                foreach (var package in settings.Packages)
                {
                    builder.AppendNamedQuoted("package", string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}:{1}", package.Key, package.Value));
                }
            }
        }
    }
}