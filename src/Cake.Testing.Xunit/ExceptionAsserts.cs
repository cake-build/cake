// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;

// ReSharper disable once CheckNamespace
namespace Xunit
{
    /// <summary>
    /// Provides additional assertion methods for testing exceptions.
    /// </summary>
    public partial class AssertEx
    {
        /// <summary>
        /// Verifies that the exception is an ArgumentNullException with the specified parameter name.
        /// </summary>
        /// <param name="exception">The exception to verify.</param>
        /// <param name="parameterName">The expected parameter name.</param>
        public static void IsArgumentNullException(Exception exception, string parameterName)
        {
            Assert.IsType<ArgumentNullException>(exception);
            Assert.Equal(parameterName, ((ArgumentNullException)exception).ParamName);
        }

        /// <summary>
        /// Verifies that the exception is an ArgumentOutOfRangeException with the specified parameter name.
        /// </summary>
        /// <param name="exception">The exception to verify.</param>
        /// <param name="parameterName">The expected parameter name.</param>
        public static void IsArgumentOutOfRangeException(Exception exception, string parameterName)
        {
            Assert.IsType<ArgumentOutOfRangeException>(exception);
            Assert.Equal(parameterName, ((ArgumentOutOfRangeException)exception).ParamName);
        }

        /// <summary>
        /// Verifies that the exception is an ArgumentException with the specified parameter name and message.
        /// </summary>
        /// <param name="exception">The exception to verify.</param>
        /// <param name="parameterName">The expected parameter name.</param>
        /// <param name="message">The expected message.</param>
        public static void IsArgumentException(Exception exception, string parameterName, string message)
        {
            Assert.IsType<ArgumentException>(exception);
            Assert.Equal(parameterName, ((ArgumentException)exception).ParamName);
            Assert.Equal(new ArgumentException(message, parameterName).Message, exception.Message);
        }

        /// <summary>
        /// Verifies that the exception is a CakeException with the specified message.
        /// </summary>
        /// <param name="exception">The exception to verify.</param>
        /// <param name="message">The expected message.</param>
        public static void IsCakeException(Exception exception, string message)
        {
            IsExceptionWithMessage<CakeException>(exception, message);
        }

        /// <summary>
        /// Verifies that the exception is of the specified type with the specified message.
        /// </summary>
        /// <typeparam name="T">The expected exception type.</typeparam>
        /// <param name="exception">The exception to verify.</param>
        /// <param name="message">The expected message.</param>
        public static void IsExceptionWithMessage<T>(Exception exception, string message)
            where T : Exception
        {
            Assert.IsType<T>(exception);
            Assert.Equal(message, exception.Message);
        }
    }
}