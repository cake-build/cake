using System;

// ReSharper disable once CheckNamespace
namespace Xunit
{
    public partial class Assert
    {
        public static void IsArgumentNullException(Exception exception, string parameterName)
        {
            IsType<ArgumentNullException>(exception);
            Equal(parameterName, ((ArgumentNullException)exception).ParamName);
        }

        public static void IsExceptionWithMessage<T>(Exception exception, string message)
            where T : Exception
        {
            IsType<T>(exception);
            Equal(message, exception.Message);
        }
    }
}
