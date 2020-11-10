using Cake.Cli;

namespace Cake.Tests.Fakes
{
    public sealed class FakeVersionResolver : IVersionResolver
    {
        private readonly string _version;
        private readonly string _product;

        public FakeVersionResolver(string version, string product)
        {
            _version = version;
            _product = product;
        }

        public string GetVersion()
        {
            return _version;
        }

        public string GetProductVersion()
        {
            return _product;
        }
    }
}
