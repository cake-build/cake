// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using Cake.Core;

// ReSharper disable once CheckNamespace
namespace Cake.Frosting.Internal
{
    internal static class FrostingTaskExtensions
    {
        public static bool IsRunOverridden(this IFrostingTask task, IFrostingContext context)
        {
            if (task.IsFrostingTask())
            {
                return task.GetType().GetMethod(nameof(FrostingTask.Run), new[] { context.GetType() }).IsOverriden();
            }

            if (task.IsAsyncFrostingTask())
            {
                return task.GetType().GetMethod(nameof(AsyncFrostingTask.RunAsync), new[] { context.GetType() }).IsOverriden();
            }

            throw new InvalidOperationException($"This method expects all {nameof(IFrostingTask)} to be instances of {nameof(FrostingTask)} or {nameof(AsyncFrostingTask)}.");
        }

        public static bool IsShouldRunOverridden(this IFrostingTask task, IFrostingContext context)
        {
            return task.GetType().GetMethod(nameof(IFrostingTask.ShouldRun), new[] { context.GetType() }).IsOverriden();
        }

        public static bool HasCompatibleContext(this IFrostingTask task, IFrostingContext context)
        {
            return context.GetType().IsConvertableTo(task.GetContextType());
        }

        public static bool IsContinueOnError(this IFrostingTask task)
        {
            return task.GetType().GetTypeInfo().GetCustomAttribute<ContinueOnErrorAttribute>() != null;
        }

        public static bool IsOnErrorOverridden(this IFrostingTask task, IFrostingContext context)
        {
            return task.GetType().GetMethod(nameof(IFrostingTask.OnError), new[] { typeof(Exception), context.GetType() }).IsOverriden();
        }

        public static bool IsFinallyOverridden(this IFrostingTask task, IFrostingContext context)
        {
            return task.GetType().GetMethod(nameof(IFrostingTask.Finally), new[] { context.GetType() }).IsOverriden();
        }

        public static Type GetContextType(this IFrostingTask task)
        {
            var baseType = task.GetType().GetTypeInfo().BaseType;
            if (baseType.IsConstructedGenericType)
            {
                if (baseType.GetGenericTypeDefinition() == typeof(FrostingTask<>) || baseType.GetGenericTypeDefinition() == typeof(AsyncFrostingTask<>))
                {
                    return baseType.GenericTypeArguments[0];
                }
            }
            return typeof(ICakeContext);
        }

        private static bool IsConvertableTo(this Type type, Type other)
        {
            return other == type || other.IsAssignableFrom(type);
        }

        private static bool IsAsyncFrostingTask(this IFrostingTask task)
        {
            var taskType = task.GetType();

            do
            {
                if (taskType.IsGenericType && taskType.GetGenericTypeDefinition() == typeof(AsyncFrostingTask<>))
                {
                    return true;
                }

                taskType = taskType.BaseType;
            }
            while (taskType != null);

            return false;
        }

        private static bool IsFrostingTask(this IFrostingTask task)
        {
            var taskType = task.GetType();

            do
            {
                if (taskType.IsGenericType && taskType.GetGenericTypeDefinition() == typeof(FrostingTask<>))
                {
                    return true;
                }

                taskType = taskType.BaseType;
            }
            while (taskType != null);

            return false;
        }
    }
}