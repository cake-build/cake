// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;

namespace Cake.Frosting.Internal
{
    internal static class TypeExtensions
    {
        public static string GetTaskName(this Type task)
        {
            if (task is null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            var attribute = task.GetCustomAttribute<TaskNameAttribute>();
            return attribute != null ? attribute.Name : task.Name;
        }
    }
}
