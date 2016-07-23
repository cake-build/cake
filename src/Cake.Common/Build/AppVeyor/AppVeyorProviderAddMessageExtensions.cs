using System;

namespace Cake.Common.Build.AppVeyor
{
    /// <summary>
    /// AddMessage extension methods for the IAppVeyorProvider
    /// </summary>
    public static class AppVeyorProviderAddMessageExtensions
    {
        /// <summary>
        /// Adds an informational message to the AppVeyor build log
        /// </summary>
        /// <param name="provider">The AppVeyor provider</param>
        /// <param name="format">The message</param>
        /// <param name="args">The args</param>
        public static void AddInformationalMessage(this IAppVeyorProvider provider, string format, params object[] args)
        {
            provider.AddMessage(string.Format(format, args));
        }

        /// <summary>
        /// Adds a warning message to the AppVeyor build log
        /// </summary>
        /// <param name="provider">The AppVeyor provider</param>
        /// <param name="format">The message</param>
        /// <param name="args">The args</param>
        public static void AddWarningMessage(this IAppVeyorProvider provider, string format, params object[] args)
        {
            provider.AddMessage(string.Format(format, args), AppVeyorMessageCategoryType.Warning);
        }

        /// <summary>
        /// Adds a warning message to the AppVeyor build log
        /// </summary>
        /// <param name="provider">The AppVeyor provider</param>
        /// <param name="format">The message</param>
        /// <param name="args">The args</param>
        public static void AddErrorMessage(this IAppVeyorProvider provider, string format, params object[] args)
        {
            provider.AddMessage(string.Format(format, args), AppVeyorMessageCategoryType.Error);
        }

        /// <summary>
        /// Adds a warning message to the AppVeyor build log
        /// </summary>
        /// <param name="provider">The AppVeyor provider</param>
        /// <param name="message">The message</param>
        /// <param name="exception">The exception</param>
        public static void AddErrorMessage(this IAppVeyorProvider provider, string message, Exception exception)
        {
            provider.AddMessage(message, AppVeyorMessageCategoryType.Error, exception.ToString());
        }
    }
}