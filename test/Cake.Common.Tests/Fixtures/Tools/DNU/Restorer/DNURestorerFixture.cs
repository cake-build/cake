using Cake.Common.Tools.DNU.Restore;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.DNU.Restorer
{
    internal sealed class DNURestorerFixture : DNUFixture<DNURestoreSettings>
    {
        public FilePath FilePath { get; set; }

        protected override void RunTool()
        {
            var tool = new DNURestorer(FileSystem, Environment, ProcessRunner, Globber);
            tool.Restore(FilePath, Settings);
        }
    }
}