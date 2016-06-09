// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cake.Core.Scripting
{
    /// <summary>
    /// Represents a script alias.
    /// </summary>
    public sealed class ScriptAlias
    {
        private readonly string _name;
        private readonly MethodInfo _method;
        private readonly ScriptAliasType _type;
        private readonly List<string> _namespaces;

        /// <summary>
        /// Gets the name of the alias.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the method associated with the alias.
        /// </summary>
        /// <value>The method associated with the alias.</value>
        public MethodInfo Method
        {
            get { return _method; }
        }

        /// <summary>
        /// Gets the alias type.
        /// </summary>
        /// <value>The alias type.</value>
        public ScriptAliasType Type
        {
            get { return _type; }
        }

        /// <summary>
        /// Gets all namespaces that the alias need to be imported.
        /// </summary>
        /// <value>
        /// The namespaces that the alias need to be imported.
        /// </value>
        public IReadOnlyList<string> Namespaces
        {
            get { return _namespaces; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptAlias"/> class.
        /// </summary>
        /// <param name="method">The method associated with the alias.</param>
        /// <param name="type">The alias type.</param>
        /// <param name="namespaces">The namespaces that the alias need to be imported.</param>
        public ScriptAlias(MethodInfo method, ScriptAliasType type, ISet<string> namespaces)
        {
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }
            _name = method.Name;
            _method = method;
            _type = type;
            _namespaces = new List<string>(namespaces ?? Enumerable.Empty<string>());
        }
    }
}
