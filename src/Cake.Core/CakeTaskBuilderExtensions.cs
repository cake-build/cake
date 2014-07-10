using System;

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

        public static CakeTaskBuilder<ActionTask> Does(this CakeTaskBuilder<ActionTask> builder,
            Action<ICakeContext> action)
        {
            builder.Task.AddAction(action);
            return builder;
        }

        
        public static CakeTaskBuilder<ActionTask> Does(this CakeTaskBuilder<ActionTask> builder, Action action, Func<Exception, bool> errorAction )
        {
            if (errorAction == null)
            {
                throw new ArgumentNullException("errorAction");
            }

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            return builder.Does(context=>action(), (context, exception)=>errorAction(exception));
        }

        public static CakeTaskBuilder<ActionTask> Does(this CakeTaskBuilder<ActionTask> builder, Action action, Func<ICakeContext, Exception, bool> errorAction )
        {
            if (errorAction == null)
            {
                throw new ArgumentNullException("errorAction");
            }

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            return builder.Does(context=>action(), errorAction);
        }

        public static CakeTaskBuilder<ActionTask> Does(this CakeTaskBuilder<ActionTask> builder, Action<ICakeContext> action, Func<ICakeContext, Exception, bool> errorAction )
        {
            if (errorAction == null)
            {
                throw new ArgumentNullException("errorAction");
            }

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            Action<ICakeContext> proxyAction = context => CreateProxyAction(action, errorAction, context);
            return builder.Does(proxyAction);
        }

        private static void CreateProxyAction(Action<ICakeContext> action, Func<ICakeContext, Exception, bool> errorAction, ICakeContext context)
        {
            try
            {
                action(context);
            }
            catch (Exception ex)
            {
                try
                {
                    if (!errorAction(context, ex))
                        throw;
                }
                catch (Exception errorEx)
                {
                    throw new AggregateException(
                        ex,
                        errorEx
                        );
                }
            }
        }

        public static CakeTaskBuilder<ActionTask> ContinueOnError(this CakeTaskBuilder<ActionTask> builder)
        {
            builder.Task.ContinueOnError = true;
            return builder;
        }

        public static CakeTaskBuilder<T> Description<T>(this CakeTaskBuilder<T> builder, string description)
            where T : CakeTask
        {
            builder.Task.Description = description;
            return builder;
        }

    }
}