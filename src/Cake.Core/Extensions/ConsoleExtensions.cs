// ReSharper disable once CheckNamespace
namespace Cake.Core
{
    public static class ConsoleExtensions
    {
        public static void WriteLine(this IConsole console)
        {
            if (console != null)
            {
                console.WriteLine(string.Empty);   
            }            
        }
    }
}
