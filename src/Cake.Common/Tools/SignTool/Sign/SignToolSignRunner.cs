using System;
using System.Globalization;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Utilities;
using File=System.IO.File;

namespace Cake.Common.Tools.SignTool.Sign
{
    /// <summary>
    /// The SignTool SIGN assembly runner
    /// </summary>
    public sealed class SignToolSignRunner : Tool<SignToolSignSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignToolSignRunner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        public SignToolSignRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner) : base(fileSystem, environment, processRunner)
        {
            _environment = environment;
        }

        /// <summary>
        /// Runs the tests in the specified assembly.
        /// </summary>
        /// <param name="assemblyPath">The assembly path.</param>
        /// <param name="settings">The settings.</param>
        public void Run(FilePath assemblyPath, SignToolSignSettings settings)
        {
            if (assemblyPath == null)
            {
                throw new ArgumentNullException("assemblyPath");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            Run(settings, GetArguments(assemblyPath, settings), settings.ToolPath);
        }

        private ToolArgumentBuilder GetArguments(FilePath assemblyPath, SignToolSignSettings settings)
        {
            if (assemblyPath==null || !File.Exists(assemblyPath.FullPath))
               throw new ArgumentException(
                    string.Format(CultureInfo.InvariantCulture, "{0}: AssemblyPath not specified or missing ({1})", GetToolName(), assemblyPath));


            if (settings.TimeStampUri==null)
               throw new ArgumentException(
                    string.Format(CultureInfo.InvariantCulture, "{0}: TimeStampUri Required but not specified.", GetToolName()));

            
            if (settings.CertPath==null || !File.Exists(settings.CertPath.FullPath))
               throw new ArgumentException(
                    string.Format(CultureInfo.InvariantCulture, "{0}: CertPath not specified or missing ({1})", GetToolName(), settings.CertPath));

            
            if (string.IsNullOrEmpty(settings.Password))
               throw new ArgumentException(
                    string.Format(CultureInfo.InvariantCulture, "{0}: Password Required but not specified.", GetToolName()));

            var builder = new ToolArgumentBuilder();

            //SIGN Command
            builder.AppendText("SIGN");


            //TimeStamp server
            builder.AppendText("/t");
            builder.AppendQuotedText(settings.TimeStampUri.AbsoluteUri);

            
            //Path to PFX Certificate
            builder.AppendText("/f");
            builder.AppendQuotedText(settings.CertPath.MakeAbsolute(_environment).FullPath);

            
            //PFX Password
            builder.AppendText("/p");
            builder.AppendQuotedText(settings.Password);

            //Target Assembly to sign
            builder.AppendQuotedText(assemblyPath.MakeAbsolute(_environment).FullPath);


            return builder;
        }

        /// <summary>
        /// Get name of tool
        /// </summary>
        /// <returns></returns>
        protected override string GetToolName()
        {
            return "SignTool SIGN";
        }

        /// <summary>
        /// Get the default path to tool
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        protected override FilePath GetDefaultToolPath(SignToolSignSettings settings)
        {
            return (settings==null ? null :  settings.ToolPath)
                ?? SignToolResolver.GetSignToolPath(_environment);
        }
    }
}
