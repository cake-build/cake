using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core.IO;

namespace Cake.XUnit
{
    public sealed class XUnitSettings
    {
        public bool ShadowCopy { get; set; }
        public DirectoryPath OutputDirectory { get; set; }
        public bool XmlReport { get; set; }
        public bool HtmlReport { get; set; }

        public XUnitSettings()
        {
            ShadowCopy = true;
        }
    }
}
