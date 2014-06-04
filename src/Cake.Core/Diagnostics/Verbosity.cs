using System.ComponentModel;

namespace Cake.Core.Diagnostics
{
    [TypeConverter(typeof(VerbosityTypeConverter))]
    public enum Verbosity
    {
        Quiet = 0,
        Minimal = 1,
        Normal = 2,
        Verbose = 3,
        Diagnostic = 4 
    }
}