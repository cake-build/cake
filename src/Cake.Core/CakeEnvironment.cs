using System;
using Cake.Core.IO;
using System.Reflection;

namespace Cake.Core
{
    public sealed class CakeEnvironment : ICakeEnvironment
    {
        public DirectoryPath WorkingDirectory
        {
            get { return Environment.CurrentDirectory; }
            set { SetWorkingDirectory(value); }
        }

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
            if (path == SpecialPath.Windows)
            {
                return new DirectoryPath(Environment.GetFolderPath(Environment.SpecialFolder.Windows));
            }
            const string format = "The special path '{0}' is not supported.";
            throw new NotSupportedException(string.Format(format, path.ToString()));
        }

        public DirectoryPath GetApplicationRoot()
        {
            var path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return new DirectoryPath(path);
        }

        public string GetEnvironmentVariable(string variable)
        {
            return Environment.GetEnvironmentVariable(variable);
        }

        private static void SetWorkingDirectory(DirectoryPath path)
        {
            if (path.IsRelative)
            {
                throw new CakeException("Working directory can not be set to a relative path.");
            }
            Environment.CurrentDirectory = path.FullPath;
        }
    }
}
