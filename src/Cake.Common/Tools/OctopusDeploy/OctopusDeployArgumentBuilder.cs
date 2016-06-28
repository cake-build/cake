// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.OctopusDeploy
{
    internal abstract class OctopusDeployArgumentBuilder<T> where T : OctopusDeploySettings
    {
        private readonly string _serverUrl;
        private readonly string _apiKey;

        protected ICakeEnvironment Environment { get; }

        protected ProcessArgumentBuilder Builder { get; }

        protected T Settings { get; }

        protected OctopusDeployArgumentBuilder(ICakeEnvironment environment, T settings)
            : this(settings.Server, settings.ApiKey, environment, settings)
        {
        }

        protected OctopusDeployArgumentBuilder(string server, string apiKey, ICakeEnvironment environment, T settings)
        {
            _serverUrl = server;
            _apiKey = apiKey;

            Environment = environment;
            Builder = new ProcessArgumentBuilder();
            Settings = settings;
        }

        protected void AppendArgumentIfNotNull(string argumentName, string value)
        {
            if (value != null)
            {
                Builder.Append("--" + argumentName);
                Builder.AppendQuoted(value);
            }
        }

        protected void AppendArgumentIfNotNull(string argumentName, FilePath value)
        {
            if (value != null)
            {
                Builder.Append("--" + argumentName);
                Builder.AppendQuoted(value.MakeAbsolute(Environment).FullPath);
            }
        }

        protected void AppendCommonArguments()
        {
            Builder.Append("--server");
            Builder.Append(_serverUrl);

            Builder.Append("--apiKey");
            Builder.AppendSecret(_apiKey);

            AppendArgumentIfNotNull("user", Settings.Username);

            if (Settings.Password != null)
            {
                Builder.Append("--pass");
                Builder.AppendQuotedSecret(Settings.Password);
            }

            AppendArgumentIfNotNull("configFile", Settings.ConfigurationFile);

            if (Settings.EnableDebugLogging)
            {
                Builder.Append("--debug");
            }

            if (Settings.IgnoreSslErrors)
            {
                Builder.Append("--ignoreSslErrors");
            }

            if (Settings.EnableServiceMessages)
            {
                Builder.Append("--enableServiceMessages");
            }
        }
    }
}