using System;
using System.Text;

internal static class Program
{
    private static void Main(string[] args)
    {
        foreach (var arg in args)
        {
            Console.WriteLine(Convert.ToBase64String(Encoding.UTF8.GetBytes(arg ?? string.Empty)));
        }
    }
}
