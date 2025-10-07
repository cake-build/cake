﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Frosting.Internal
{
    internal static class TaskNameHelper
    {
        public static string GetTaskName(this ITaskDependency dependency)
        {
            ArgumentNullException.ThrowIfNull(dependency);

            return dependency.Task.GetTaskName();
        }

        public static string GetTaskName(this IReverseTaskDependency dependency)
        {
            ArgumentNullException.ThrowIfNull(dependency);

            return dependency.Task.GetTaskName();
        }
    }
}
