// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;

namespace Cake.Frosting.Internal
{
    internal static class TaskNameHelper
    {
        public static string GetTaskName(IFrostingTask task)
        {
            return GetTaskName(task.GetType());
        }

        public static string GetTaskName(DependencyAttribute attribute)
        {
            return GetTaskName(attribute.Task);
        }

        public static string GetTaskName(Type task)
        {
            var attribute = task.GetTypeInfo().GetCustomAttribute<TaskNameAttribute>();
            return attribute != null ? attribute.Name : task.Name;
        }
    }
}
