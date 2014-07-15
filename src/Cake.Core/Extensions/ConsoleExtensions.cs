namespace Cake.Core.Extensions
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
