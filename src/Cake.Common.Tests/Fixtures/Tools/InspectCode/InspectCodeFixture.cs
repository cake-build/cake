using Cake.Common.Tools.InspectCode;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.InspectCode
{
    internal abstract class InspectCodeFixture : ToolFixture<InspectCodeSettings>
    {
        protected InspectCodeFixture()
            : base("inspectcode.exe")
        {
        }
    }
}