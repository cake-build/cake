using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Testing.Shared
{
    public abstract class ToolFixture<TToolSettings> : ToolFixture<TToolSettings, ToolFixtureResult>
        where TToolSettings : ToolSettings, new()
    {
        protected ToolFixture(string toolFilename)
            : base(toolFilename)
        {
        }

        protected sealed override ToolFixtureResult CreateResult(FilePath toolPath, ProcessSettings process)
        {
            return new ToolFixtureResult(toolPath, process);
        }
    }
}
