using System;

namespace Cake.Core
{
    public static partial class CakeTaskBuilderExtensions
    {
        /// <summary>
        /// Adds a criteria that has to be fulfilled for the task to run.
        /// </summary>
        /// <param name="builder">The task builder.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder WithCriteria(this CakeTaskBuilder builder, bool criteria)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Target.AddCriteria(_ => criteria);
            return builder;
        }

        /// <summary>
        /// Adds a criteria that has to be fulfilled for the task to run.
        /// </summary>
        /// <param name="builder">The task builder.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="message">The message to display if the task was skipped due to the provided criteria.</param>
        /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder WithCriteria(this CakeTaskBuilder builder, bool criteria, string message)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Target.AddCriteria(_ => criteria, message);
            return builder;
        }

        /// <summary>
        /// Adds a criteria that has to be fulfilled for the task to run.
        /// The criteria is evaluated when traversal of the graph occurs.
        /// </summary>
        /// <param name="builder">The task builder.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder WithCriteria(this CakeTaskBuilder builder, Func<bool> criteria)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria));
            }

            builder.Target.AddCriteria(_ => criteria());
            return builder;
        }

        /// <summary>
        /// Adds a criteria that has to be fulfilled for the task to run.
        /// The criteria is evaluated when traversal of the graph occurs.
        /// </summary>
        /// <param name="builder">The task builder.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="message">The message to display if the task was skipped due to the provided criteria.</param>
        /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder WithCriteria(this CakeTaskBuilder builder, Func<bool> criteria, string message)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria));
            }

            builder.Target.AddCriteria(_ => criteria(), message);
            return builder;
        }

        /// <summary>
        /// Adds a criteria that has to be fulfilled for the task to run.
        /// The criteria is evaluated when traversal of the graph occurs.
        /// </summary>
        /// <param name="builder">The task builder.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder WithCriteria(this CakeTaskBuilder builder, Func<ICakeContext, bool> criteria)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria));
            }

            builder.Target.AddCriteria(criteria);
            return builder;
        }

        /// <summary>
        /// Adds a criteria that has to be fulfilled for the task to run.
        /// The criteria is evaluated when traversal of the graph occurs.
        /// </summary>
        /// <param name="builder">The task builder.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="message">The message to display if the task was skipped due to the provided criteria.</param>
        /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder WithCriteria(this CakeTaskBuilder builder, Func<ICakeContext, bool> criteria, string message)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria));
            }

            builder.Target.AddCriteria(criteria, message);
            return builder;
        }

        /// <summary>
        /// Adds a criteria that has to be fulfilled for the task to run.
        /// The criteria is evaluated when traversal of the graph occurs.
        /// </summary>
        /// <typeparam name="TData">The type of the data context.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder WithCriteria<TData>(this CakeTaskBuilder builder, Func<TData, bool> criteria) where TData : class
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria));
            }

            builder.Target.AddCriteria(context => criteria(context.Data.Get<TData>()));
            return builder;
        }

        /// <summary>
        /// Adds a criteria that has to be fulfilled for the task to run.
        /// The criteria is evaluated when traversal of the graph occurs.
        /// </summary>
        /// <typeparam name="TData">The type of the data context.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="message">The message to display if the task was skipped due to the provided criteria.</param>
        /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder WithCriteria<TData>(this CakeTaskBuilder builder, Func<TData, bool> criteria, string message) where TData : class
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria));
            }

            builder.Target.AddCriteria(context => criteria(context.Data.Get<TData>()), message);
            return builder;
        }

        /// <summary>
        /// Adds a criteria that has to be fulfilled for the task to run.
        /// The criteria is evaluated when traversal of the graph occurs.
        /// </summary>
        /// <typeparam name="TData">The type of the data context.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder WithCriteria<TData>(this CakeTaskBuilder builder, Func<ICakeContext, TData, bool> criteria) where TData : class
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria));
            }

            builder.Target.AddCriteria(context => criteria(context, context.Data.Get<TData>()));
            return builder;
        }

        /// <summary>
        /// Adds a criteria that has to be fulfilled for the task to run.
        /// The criteria is evaluated when traversal of the graph occurs.
        /// </summary>
        /// <typeparam name="TData">The type of the data context.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="message">The message to display if the task was skipped due to the provided criteria.</param>
        /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder WithCriteria<TData>(this CakeTaskBuilder builder, Func<ICakeContext, TData, bool> criteria, string message) where TData : class
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria));
            }

            builder.Target.AddCriteria(context => criteria(context, context.Data.Get<TData>()), message);
            return builder;
        }
    }
}
