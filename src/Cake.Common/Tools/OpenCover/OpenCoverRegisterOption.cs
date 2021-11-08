// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

using Cake.Core.IO;

namespace Cake.Common.Tools.OpenCover
{
    /// <summary>
    /// Represents the register-options:
    /// <list type="bullet">
    /// <item>
    /// <term>empty</term>
    /// <description>Registers and de-register the code coverage profiler. (Administrative permissions to the registry are required.)</description>
    /// </item>
    /// <item>
    /// <term>"user"</term>
    /// <description>Does per-user registration where the user account does not have administrative permissions.</description>
    /// </item>
    /// <item>
    /// <term>path</term>
    /// <description>If you do not want to use the registry entries, use -register:path to select the profiler.</description>
    /// </item>
    /// </list>
    /// </summary>
    public abstract class OpenCoverRegisterOption
    {
        private readonly string commandLineValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenCoverRegisterOption"/> class.
        /// </summary>
        /// <param name="commandLineValue">The value, as required for the commandline.</param>
        /// <remarks>
        /// Note that no value (<c>null</c> or <c>string.Empty</c>) is valid.
        /// However, if a value exists it NEEDS to start with a colon (":").
        /// </remarks>
        protected OpenCoverRegisterOption(string commandLineValue)
        {
            if (string.IsNullOrEmpty(commandLineValue))
            {
                commandLineValue = string.Empty;
            }

            if (!string.IsNullOrEmpty(commandLineValue))
            {
                if (!commandLineValue.StartsWith(":", StringComparison.Ordinal))
                {
                    throw new ArgumentException(nameof(commandLineValue), "if a non-empty value is given, it needs to start with ':'");
                }
            }

            this.commandLineValue = commandLineValue;
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return commandLineValue;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="OpenCoverRegisterOption"/>.
        /// (Since the switch from pure string to <see cref="OpenCoverRegisterOption"/> is a breaking change.)
        /// </summary>
        /// <param name="option">The option.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [Obsolete("use new OpenCoverRegisterOption() instead.")]
        public static implicit operator OpenCoverRegisterOption(string option)
        {
            if (string.IsNullOrEmpty(option))
            {
                return new OpenCoverRegisterOptionAdmin();
            }

            if (option.Equals("user", StringComparison.InvariantCultureIgnoreCase))
            {
                return new OpenCoverRegisterOptionUser();
            }

            return new OpenCoverRegisterOptionDll(new FilePath(option));
        }
    }

    /// <summary>
    /// Gets the register-option representing the "user"-mode.
    /// (This will translate to "-register:user".)
    /// </summary>
    /// <seealso cref="OpenCoverRegisterOption" />
    public class OpenCoverRegisterOptionUser
        : OpenCoverRegisterOption
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenCoverRegisterOptionUser"/> class.
        /// </summary>
        public OpenCoverRegisterOptionUser()
            : base(":user")
        {
        }
    }

    /// <summary>
    /// Gets the register-option representing the "admin"-mode.
    /// (This will translate to "-register".)
    /// </summary>
    /// <seealso cref="OpenCoverRegisterOption" />
    public class OpenCoverRegisterOptionAdmin
        : OpenCoverRegisterOption
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenCoverRegisterOptionAdmin"/> class.
        /// </summary>
        public OpenCoverRegisterOptionAdmin()
            : base(string.Empty)
        {
        }
    }

    /// <summary>
    /// Gets a register-option pointing to a dll.
    /// (This will translate to "-register:[path-to-dll]".)
    /// </summary>
    /// <seealso cref="OpenCoverRegisterOption" />
    public class OpenCoverRegisterOptionDll
        : OpenCoverRegisterOption
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenCoverRegisterOptionDll"/> class.
        /// </summary>
        /// <param name="pathToDll">Path to the dll.</param>
        public OpenCoverRegisterOptionDll(FilePath pathToDll)
            : base($":{pathToDll.FullPath}")
        {
        }
    }
}