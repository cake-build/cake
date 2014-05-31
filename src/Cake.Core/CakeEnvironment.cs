using System;
using Cake.Core.IO;

namespace Cake.Core
{
    internal sealed class CakeEnvironment : ICakeEnvironment
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
    }
}
