using System.IO;
using System.Linq;
using System.Reflection;
using Cake.Core.Scripting.CodeGen;
using Cake.Core.Tests.Data;

namespace Cake.Core.Tests.Fixtures
{
    public sealed class MethodAliasGeneratorFixture
    {
        private readonly Assembly _assembly;
        private readonly MethodInfo[] _methods;

        public MethodAliasGeneratorFixture()
        {
            _assembly = typeof (MethodAliasGeneratorFixture).Assembly;
            _methods = typeof (MethodAliasGeneratorData).GetMethods();
        }

        public string GetExpectedCode(string name)
        {
            var resource = string.Concat("Cake.Core.Tests.Unit.Scripting.CodeGen.Expected.Methods.", name);
            using (var stream = _assembly.GetManifestResourceStream(resource))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd().NormalizeGeneratedCode();
            }
        }

        public string Generate(string name)
        {
            var method = _methods.SingleOrDefault(x => x.Name == name);
            return MethodAliasGenerator.Generate(method).NormalizeGeneratedCode();
        }
    }
}
