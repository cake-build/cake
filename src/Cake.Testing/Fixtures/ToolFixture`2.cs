// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Linq;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Testing.Fixtures
{
    /// <summary>
    /// Base class for tool fixtures.
    /// </summary>
    /// <typeparam name="TToolSettings">The type of the tool settings.</typeparam>
    /// <typeparam name="TFixtureResult">The type of the fixture result.</typeparam>
    public abstract class ToolFixture<TToolSettings, TFixtureResult>
        where TToolSettings : ToolSettings, new()
        where TFixtureResult : ToolFixtureResult
    {
        private readonly FilePath _defaultToolPath;

        /// <summary>
        /// Gets or sets the file system.
        /// </summary>
        /// <value>The file system.</value>
        public FakeFileSystem FileSystem { get; set; }

        /// <summary>
        /// Gets or sets the process runner.
        /// </summary>
        /// <value>The process runner.</value>
        public ToolFixtureProcessRunner<TFixtureResult> ProcessRunner { get; set; }

        /// <summary>
        /// Gets or sets the environment.
        /// </summary>
        /// <value>The environment.</value>
        public FakeEnvironment Environment { get; set; }

        /// <summary>
        /// Gets or sets the globber.
        /// </summary>
        /// <value>The globber.</value>
        public IGlobber Globber { get; set; }

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public FakeConfiguration Configuration { get; set; }

        /// <summary>
        /// Gets or sets the tool locator.
        /// </summary>
        /// <value>The tool locator.</value>
        public IToolLocator Tools { get; set; }

        /// <summary>
        /// Gets or sets the tool settings.
        /// </summary>
        /// <value>The tool settings.</value>
        public TToolSettings Settings { get; set; }

        /// <summary>
        /// Gets the default tool path.
        /// </summary>
        /// <value>The default tool path.</value>
        public FilePath DefaultToolPath
        {
            get { return _defaultToolPath; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolFixture{TToolSettings, TFixtureResult}"/> class.
        /// </summary>
        /// <param name="toolFilename">The tool filename.</param>
        protected ToolFixture(string toolFilename)
        {
            Settings = new TToolSettings();
            ProcessRunner = new ToolFixtureProcessRunner<TFixtureResult>(CreateResult);
            Environment = FakeEnvironment.CreateUnixEnvironment();
            FileSystem = new FakeFileSystem(Environment);
            Globber = new Globber(FileSystem, Environment);
            Configuration = new FakeConfiguration();
            Tools = new ToolLocator(Environment, new ToolRepository(Environment), new ToolResolutionStrategy(FileSystem, Environment, Globber, Configuration));

            // ReSharper disable once VirtualMemberCallInContructor
            _defaultToolPath = GetDefaultToolPath(toolFilename);
            FileSystem.CreateFile(_defaultToolPath);
        }

        /// <summary>
        /// Gets the default tool path.
        /// </summary>
        /// <param name="toolFilename">The tool filename.</param>
        /// <returns>The default tool path.</returns>
        protected virtual FilePath GetDefaultToolPath(string toolFilename)
        {
            return new FilePath("./tools/" + toolFilename).MakeAbsolute(Environment);
        }

        /// <summary>
        /// Runs the tool.
        /// </summary>
        /// <returns>The result from running the tool.</returns>
        public TFixtureResult Run()
        {
            // Run the tool.
            RunTool();

            // Returned the intercepted result.
            return ProcessRunner.Results.LastOrDefault();
        }

        /// <summary>
        /// Creates the tool fixture result from the provided
        /// tool path and process settings.
        /// </summary>
        /// <param name="path">The tool path.</param>
        /// <param name="process">The process settings.</param>
        /// <returns>A tool fixture result.</returns>
        protected abstract TFixtureResult CreateResult(FilePath path, ProcessSettings process);

        /// <summary>
        /// Runs the tool.
        /// </summary>
        protected abstract void RunTool();
    }
}
