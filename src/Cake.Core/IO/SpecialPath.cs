// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Core.IO
{
    /// <summary>
    /// Represents a special path.
    /// </summary>
    public enum SpecialPath
    {
        /// <summary>
        /// The directory that serves as a common repository for application-specific
        /// data for the current roaming user.
        /// </summary>
        ApplicationData,

        /// <summary>
        /// The directory that serves as a common repository for application-specific
        /// data that is used by all users.
        /// </summary>
        CommonApplicationData,

        /// <summary>
        /// The directory that serves as a common repository for application-specific
        /// data that is used by the current, non-roaming user.
        /// </summary>
        LocalApplicationData,

        /// <summary>
        /// The Program Files folder.
        /// </summary>
        ProgramFiles,

        /// <summary>
        /// The Program Files (X86) folder.
        /// </summary>
        ProgramFilesX86,

        /// <summary>
        /// The Windows folder.
        /// </summary>
        Windows,

        /// <summary>
        /// The current user's temporary folder.
        /// </summary>
        LocalTemp
    }
}
