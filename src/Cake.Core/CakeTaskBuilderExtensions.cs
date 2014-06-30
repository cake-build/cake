﻿using System;
using System.ComponentModel;

namespace Cake.Core
{
    public static class CakeTaskBuilderExtensions
    {
        public static CakeTaskBuilder<T> IsDependentOn<T>(this CakeTaskBuilder<T> builder, string name)
            where T : CakeTask
        {
            builder.Task.AddDependency(name);
            return builder;
        }

        public static CakeTaskBuilder<T> WithCriteria<T>(this CakeTaskBuilder<T> builder, bool criteria)
            where T : CakeTask
        {
            builder.Task.AddCriteria(() => criteria);
            return builder;
        }

        public static CakeTaskBuilder<T> WithCriteria<T>(this CakeTaskBuilder<T> builder, Func<bool> criteria)
            where T : CakeTask
        {
            builder.Task.AddCriteria(criteria);
            return builder;
        }

        public static CakeTaskBuilder<ActionTask> Does(this CakeTaskBuilder<ActionTask> builder, Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            return Does(builder, context => action());
        }

        public static CakeTaskBuilder<ActionTask> Does(this CakeTaskBuilder<ActionTask> builder, Action<ICakeContext> action)
        {
            builder.Task.AddAction(action);
            return builder;
        }

        public static CakeTaskBuilder<ActionTask> ContinueOnError(this CakeTaskBuilder<ActionTask> builder)
        {
            builder.Task.ContinueOnError = true;
            return builder;
        }

        public static CakeTaskBuilder<T> DescriptionAttribute<T>(this CakeTaskBuilder<T> builder, string description)
            where T : CakeTask
        {
            return builder;
        }

    }
}