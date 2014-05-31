using System;
using System.Runtime.Serialization;

namespace Cake.Core
{
    [Serializable]
    public sealed class CakeException : Exception
    {
        public CakeException()
        {            
        }

        public CakeException(string message)
            : base(message)
        {
        }

        public CakeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        private CakeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
