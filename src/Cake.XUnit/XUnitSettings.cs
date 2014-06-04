using System;
using Cake.Core.IO;

namespace Cake.XUnit
{
    public sealed class XUnitSettings
    {
        private readonly FilePath _assembly;

        public FilePath Assembly
        {
            get { return _assembly; }
        }

        public XUnitSettings(FilePath assemblyPath)
        {
            if (assemblyPath == null)
            {
                throw new ArgumentNullException("assemblyPath");
            }
            _assembly = assemblyPath;
        }
    }
}
