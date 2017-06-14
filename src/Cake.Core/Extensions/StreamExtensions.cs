using System.IO;

// ReSharper disable once CheckNamespace
namespace Cake.Core
{
    internal static class StreamExtensions
    {
        public static byte[] ToArray(this Stream source)
        {
            if (source is MemoryStream memoryStream)
            {
                return memoryStream.ToArray();
            }

            var buffer = new byte[16 * 1024];
            using (var destination = new MemoryStream())
            {
                int read;
                while ((read = source.Read(buffer, 0, buffer.Length)) > 0)
                {
                    destination.Write(buffer, 0, read);
                }
                return destination.ToArray();
            }
        }
    }
}
