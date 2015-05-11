using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.OctopusDeploy
{
    internal class CreateReleaseArgumentBuilder
    {
        private readonly string _projectName;
        private readonly string _server;
        private readonly string _apiKey;
        private readonly CreateReleaseSettings _settings;
        private readonly ICakeEnvironment _environment;

        private readonly ProcessArgumentBuilder _builder;

        public CreateReleaseArgumentBuilder(string projectName, string server, string apiKey, CreateReleaseSettings settings, ICakeEnvironment environment)
        {
            _projectName = projectName;
            _server = server;
            _apiKey = apiKey;
            _settings = settings;
            _environment = environment;
            _builder = new ProcessArgumentBuilder();
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
            _builder.Append("create-release");

            _builder.Append("--project");
            _builder.AppendQuoted(_projectName);

            _builder.Append("--server");
            _builder.Append(_server);

            _builder.Append("--apiKey");
            _builder.AppendSecret(_apiKey);

            AppendArgumentIfNotNull("username", _settings.Username);

            if (_settings.Password != null)
            {
                _builder.Append("--" + "password");
                _builder.AppendQuoted(_settings.Password);
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
                _builder.Append("--" + argumentName);
                _builder.AppendQuoted(value);
            }
        }

        private void AppendArgumentIfNotNull(string argumentName, FilePath value)
        {
            if (value != null)
            {
                _builder.Append("--" + argumentName);
                _builder.AppendQuoted(value.MakeAbsolute(_environment).FullPath);
            }
        }

        private static void AppendPackages(CreateReleaseSettings settings, ProcessArgumentBuilder builder)
        {
            if (settings.Packages != null)
            {
                foreach (var package in settings.Packages)
                {
                    builder.Append("--package");
                    builder.AppendQuoted(string.Format("{0}:{1}", package.Key, package.Value));
                }
            }
        }
    }
}