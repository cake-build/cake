using Cake.Core.IO;

namespace Cake.Testing.Shared
{
    public class ToolFixtureResult
    {
        private readonly FilePath _toolPath;
        private readonly ProcessSettings _process;
        private readonly string _args;

        public FilePath ToolPath
        {
            get { return _toolPath; }
        }

        public ProcessSettings Process
        {
            get { return _process; }
        }

        public string Args
        {
            get { return _args; }
        }

        public ToolFixtureResult(FilePath toolPath, ProcessSettings process)
        {
            _toolPath = toolPath;
            _args = process.Arguments.Render();
            _process = process;
        }
    }
}