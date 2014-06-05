using System.ComponentModel;
using Cake.Core;

namespace Cake.Scripting
{
    public sealed partial class ScriptHost
    {
        public bool HasArgument(string key)
        {
            return Arguments.HasArgument(key);
        }

        public T Argument<T>(string key)
        {
            var value = Arguments.GetArgument(key);
            if (value == null)
            {
                throw new CakeException("Argument was not set.");
            }
            return Convert<T>(value);
        }

        public T Argument<T>(string key, T defaultValue)
        {
            var value = Arguments.GetArgument(key);
            return value == null
                ? defaultValue
                : Convert<T>(value);
        }

        private static T Convert<T>(string value)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)converter.ConvertFromInvariantString(value);
        }
    }
}
