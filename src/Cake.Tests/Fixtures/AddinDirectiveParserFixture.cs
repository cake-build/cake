using Cake.Core;
using Cake.Core.IO;
using Cake.Testing.Fakes;
using NSubstitute;

namespace Cake.Tests.Fixtures
{
  public class AddInDirectiveParserFixture
  {
    public FakeLog Log { get; set; }
    public IFileSystem FileSystem { get; set; }
    public ICakeEnvironment Environment { get; set; }

    public AddInDirectiveParserFixture()
    {
      Log = new FakeLog();
      Environment = Substitute.For<ICakeEnvironment>();
      Environment.GetApplicationRoot().Returns(e => "c:/MyCakeFolder");
      Environment.WorkingDirectory.Returns(e => "c:/MyCakeFolder");

      FileSystem = new FakeFileSystem(Environment);
    }
  }
}
