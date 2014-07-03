using Cake.Core.IO;

namespace Cake.Core
{
    public interface ICakeEnvironment
    {
        DirectoryPath WorkingDirectory { get; set; }
        bool Is64BitOperativeSystem();
        bool IsUnix();        
        DirectoryPath GetSpecialPath(SpecialPath path);
        DirectoryPath GetApplicationRoot();
        string GetEnvironmentVariable(string variable);
    }
}
