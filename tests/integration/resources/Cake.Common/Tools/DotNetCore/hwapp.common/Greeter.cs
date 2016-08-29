using System;

namespace HWApp.Common
{
    public static class Greeter
    {
        public static string GetGreeting(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            return $"Hello {name}!";
        }
    }
}
