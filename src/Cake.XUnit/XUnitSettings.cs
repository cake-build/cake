using System;
using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.XUnit
{
    public sealed class XUnitSettings
    {
        private readonly List<FilePath> _assemblies;

        public XUnitSettings(IEnumerable<FilePath> assemblyPaths)
        {
            if (assemblyPaths == null)
            {
                throw new ArgumentNullException("assemblyPaths");
            }
            _assemblies = new List<FilePath>(assemblyPaths);
        }

        public IEnumerable<FilePath> GetAssemblyPaths()
        {
            return _assemblies;
        }
    }
}
