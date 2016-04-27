namespace Cake.Core.Configuration
{
    internal static class KeyNormalizer
    {
        public static string Normalize(string key)
        {
            key = key.ToUpperInvariant();
            return key.Trim();
        }
    }
}
