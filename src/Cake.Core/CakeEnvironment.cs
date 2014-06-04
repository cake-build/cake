using System;
using Cake.Core.IO;
using System.Reflection;

namespace Cake.Core
{
    public sealed class CakeEnvironment : ICakeEnvironment
    {
        public DirectoryPath WorkingDirectory { get; set; }

        public CakeEnvironment()
        {
            WorkingDirectory = new DirectoryPath(Environment.CurrentDirectory);    
        }

        public bool Is64BitOperativeSystem()
        {
            return Machine.Is64BitOperativeSystem();
        }

        public bool IsUnix()
        {
            return Machine.IsUnix();
        }

        public DirectoryPath GetSpecialPath(SpecialPath path)
        {
            if (path == SpecialPath.ProgramFilesX86)
            {
                return new DirectoryPath(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86));
            }
            throw new NotSupportedException();
        }

        public DirectoryPath GetApplicationRoot()
        {
            var path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return new DirectoryPath(path);
        }
    }
}
