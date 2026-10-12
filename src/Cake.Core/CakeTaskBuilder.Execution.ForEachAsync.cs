// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cake.Core;

public static partial class CakeTaskBuilderExtensions
{
    /// <summary>
    /// Adds an asynchronous action to be executed foreach item in the list.
    /// </summary>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="builder">The task builder.</param>
    /// <param name="items">The items.</param>
    /// <param name="action">The action.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Pack")
    ///     .DoesForEach(new[] { "net10.0", "net11.0" }, async tfm =>
    /// {
    ///     await System.Threading.Tasks.Task.Delay(10);
    ///     Information("Packing {0}", tfm);
    /// });
    /// </code>
    /// </example>
    public static CakeTaskBuilder DoesForEach<TItem>(this CakeTaskBuilder builder, IEnumerable<TItem> items, Func<TItem, Task> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        return DoesForEach(builder, items, (item, _) => action(item));
    }

    /// <summary>
    /// Adds an asynchronous action to be executed foreach item in the list.
    /// </summary>
    /// <typeparam name="TData">The type of the data context.</typeparam>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="builder">The task builder.</param>
    /// <param name="items">The items.</param>
    /// <param name="action">The action.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Pack")
    ///     .DoesForEach&lt;Foo, string&gt;(new[] { "net10.0" }, async (data, tfm) =>
    /// {
    ///     await System.Threading.Tasks.Task.Delay(10);
    ///     Information("Packing {0} for {1}", tfm, data.Place);
    /// });
    /// </code>
    /// </example>
    public static CakeTaskBuilder DoesForEach<TData, TItem>(this CakeTaskBuilder builder, IEnumerable<TItem> items, Func<TData, TItem, Task> action) where TData : class
    {
        ArgumentNullException.ThrowIfNull(action);

        return DoesForEach<TData, TItem>(builder, items, (data, item, _) => action(data, item));
    }

    /// <summary>
    /// Adds an asynchronous action to be executed foreach item in the list.
    /// </summary>
    /// <typeparam name="TData">The type of the data context.</typeparam>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="builder">The task builder.</param>
    /// <param name="items">The items.</param>
    /// <param name="action">The action.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Pack")
    ///     .DoesForEach&lt;Foo, string&gt;(new[] { "net10.0" }, async (data, tfm, context) =>
    /// {
    ///     await System.Threading.Tasks.Task.Delay(10);
    ///     context.Log.Information("Packing {0} for {1}", tfm, data.Place);
    /// });
    /// </code>
    /// </example>
    public static CakeTaskBuilder DoesForEach<TData, TItem>(this CakeTaskBuilder builder, IEnumerable<TItem> items, Func<TData, TItem, ICakeContext, Task> action) where TData : class
    {
        ArgumentNullException.ThrowIfNull(action);

        return DoesForEach(builder, items, (item, context) => action(context.Data.Get<TData>(), item, context));
    }

    /// <summary>
    /// Adds an asynchronous action to be executed foreach item in the list.
    /// </summary>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="builder">The task builder.</param>
    /// <param name="items">The items.</param>
    /// <param name="action">The action.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Pack")
    ///     .DoesForEach(new[] { "net10.0" }, async (tfm, context) =>
    /// {
    ///     await System.Threading.Tasks.Task.Delay(10);
    ///     context.Log.Information("Packing {0}", tfm);
    /// });
    /// </code>
    /// </example>
    public static CakeTaskBuilder DoesForEach<TItem>(this CakeTaskBuilder builder, IEnumerable<TItem> items, Func<TItem, ICakeContext, Task> action)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(action);

        foreach (var item in items)
        {
            builder.Target.AddAction(context => action(item, context));
        }

        return builder;
    }

    /// <summary>
    /// Adds an asynchronous action to be executed foreach item returned by the items function.
    /// This method will be executed the first time the task is executed.
    /// </summary>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="builder">The task builder.</param>
    /// <param name="itemsFunc">The items.</param>
    /// <param name="action">The action.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Pack")
    ///     .DoesForEach(() => new[] { "net10.0" }, async tfm =>
    /// {
    ///     await System.Threading.Tasks.Task.Delay(10);
    ///     Information("Packing {0}", tfm);
    /// });
    /// </code>
    /// </example>
    public static CakeTaskBuilder DoesForEach<TItem>(this CakeTaskBuilder builder, Func<IEnumerable<TItem>> itemsFunc, Func<TItem, Task> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        return DoesForEach(builder, _ => itemsFunc(), (item, _) => action(item));
    }

    /// <summary>
    /// Adds an asynchronous action to be executed foreach item returned by the items function.
    /// This method will be executed the first time the task is executed.
    /// </summary>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="builder">The task builder.</param>
    /// <param name="itemsFunc">The items.</param>
    /// <param name="action">The action.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Pack")
    ///     .DoesForEach(context => new[] { "net10.0" }, async tfm =>
    /// {
    ///     await System.Threading.Tasks.Task.Delay(10);
    ///     Information("Packing {0}", tfm);
    /// });
    /// </code>
    /// </example>
    public static CakeTaskBuilder DoesForEach<TItem>(this CakeTaskBuilder builder, Func<ICakeContext, IEnumerable<TItem>> itemsFunc, Func<TItem, Task> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        return DoesForEach(builder, itemsFunc, (item, _) => action(item));
    }

    /// <summary>
    /// Adds an asynchronous action to be executed foreach item returned by the items function.
    /// This method will be executed the first time the task is executed.
    /// </summary>
    /// <typeparam name="TData">The type of the data context.</typeparam>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="builder">The task builder.</param>
    /// <param name="itemsFunc">The items.</param>
    /// <param name="action">The action.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Pack")
    ///     .DoesForEach&lt;Foo, string&gt;(() => new[] { "net10.0" }, async (data, tfm) =>
    /// {
    ///     await System.Threading.Tasks.Task.Delay(10);
    ///     Information("Packing {0} for {1}", tfm, data.Place);
    /// });
    /// </code>
    /// </example>
    public static CakeTaskBuilder DoesForEach<TData, TItem>(this CakeTaskBuilder builder, Func<IEnumerable<TItem>> itemsFunc, Func<TData, TItem, Task> action) where TData : class
    {
        ArgumentNullException.ThrowIfNull(action);

        return DoesForEach<TData, TItem>(builder, itemsFunc, (data, item, _) => action(data, item));
    }

    /// <summary>
    /// Adds an asynchronous action to be executed foreach item returned by the items function.
    /// This method will be executed the first time the task is executed.
    /// </summary>
    /// <typeparam name="TData">The type of the data context.</typeparam>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="builder">The task builder.</param>
    /// <param name="itemsFunc">The items.</param>
    /// <param name="action">The action.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Pack")
    ///     .DoesForEach&lt;Foo, string&gt;(() => new[] { "net10.0" }, async (data, tfm, context) =>
    /// {
    ///     await System.Threading.Tasks.Task.Delay(10);
    ///     context.Log.Information("Packing {0} for {1}", tfm, data.Place);
    /// });
    /// </code>
    /// </example>
    public static CakeTaskBuilder DoesForEach<TData, TItem>(this CakeTaskBuilder builder, Func<IEnumerable<TItem>> itemsFunc, Func<TData, TItem, ICakeContext, Task> action) where TData : class
    {
        ArgumentNullException.ThrowIfNull(action);

        return DoesForEach(builder, _ => itemsFunc(), (item, context) => action(context.Data.Get<TData>(), item, context));
    }

    /// <summary>
    /// Adds an asynchronous action to be executed foreach item returned by the items function.
    /// This method will be executed the first time the task is executed.
    /// </summary>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="builder">The task builder.</param>
    /// <param name="itemsFunc">The items.</param>
    /// <param name="action">The action.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Pack")
    ///     .DoesForEach(() => new[] { "net10.0" }, async (tfm, context) =>
    /// {
    ///     await System.Threading.Tasks.Task.Delay(10);
    ///     context.Log.Information("Packing {0}", tfm);
    /// });
    /// </code>
    /// </example>
    public static CakeTaskBuilder DoesForEach<TItem>(this CakeTaskBuilder builder, Func<IEnumerable<TItem>> itemsFunc, Func<TItem, ICakeContext, Task> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        return DoesForEach(builder, _ => itemsFunc(), action);
    }

    /// <summary>
    /// Adds an asynchronous action to be executed foreach item returned by the items function.
    /// This method will be executed the first time the task is executed.
    /// </summary>
    /// <typeparam name="TData">The type of the data context.</typeparam>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="builder">The task builder.</param>
    /// <param name="itemsFunc">The items.</param>
    /// <param name="action">The action.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Pack")
    ///     .DoesForEach&lt;Foo, string&gt;(context => new[] { "net10.0" }, async (data, tfm) =>
    /// {
    ///     await System.Threading.Tasks.Task.Delay(10);
    ///     Information("Packing {0} for {1}", tfm, data.Place);
    /// });
    /// </code>
    /// </example>
    public static CakeTaskBuilder DoesForEach<TData, TItem>(this CakeTaskBuilder builder, Func<ICakeContext, IEnumerable<TItem>> itemsFunc, Func<TData, TItem, Task> action) where TData : class
    {
        ArgumentNullException.ThrowIfNull(action);

        return DoesForEach(builder, itemsFunc, (item, context) => action(context.Data.Get<TData>(), item));
    }

    /// <summary>
    /// Adds an asynchronous action to be executed foreach item returned by the items function.
    /// This method will be executed the first time the task is executed.
    /// </summary>
    /// <typeparam name="TData">The type of the data context.</typeparam>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="builder">The task builder.</param>
    /// <param name="itemsFunc">The items.</param>
    /// <param name="action">The action.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Pack")
    ///     .DoesForEach&lt;Foo, string&gt;(data => data.Frameworks, async (data, tfm, context) =>
    /// {
    ///     await System.Threading.Tasks.Task.Delay(10);
    ///     context.Log.Information("Packing {0} for {1}", tfm, data.Place);
    /// });
    /// </code>
    /// </example>
    public static CakeTaskBuilder DoesForEach<TData, TItem>(this CakeTaskBuilder builder, Func<TData, IEnumerable<TItem>> itemsFunc, Func<TData, TItem, ICakeContext, Task> action) where TData : class
    {
        ArgumentNullException.ThrowIfNull(action);

        return DoesForEach(builder, context => itemsFunc(context.Data.Get<TData>()), (item, context) => action(context.Data.Get<TData>(), item, context));
    }

    /// <summary>
    /// Adds an asynchronous action to be executed foreach item returned by the items function.
    /// This method will be executed the first time the task is executed.
    /// </summary>
    /// <typeparam name="TData">The type of the data context.</typeparam>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="builder">The task builder.</param>
    /// <param name="itemsFunc">The items.</param>
    /// <param name="action">The action.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Pack")
    ///     .DoesForEach&lt;Foo, string&gt;(context => new[] { "net10.0" }, async (data, tfm, context) =>
    /// {
    ///     await System.Threading.Tasks.Task.Delay(10);
    ///     context.Log.Information("Packing {0} for {1}", tfm, data.Place);
    /// });
    /// </code>
    /// </example>
    public static CakeTaskBuilder DoesForEach<TData, TItem>(this CakeTaskBuilder builder, Func<ICakeContext, IEnumerable<TItem>> itemsFunc, Func<TData, TItem, ICakeContext, Task> action) where TData : class
    {
        ArgumentNullException.ThrowIfNull(action);

        return DoesForEach(builder, itemsFunc, (item, context) => action(context.Data.Get<TData>(), item, context));
    }

    /// <summary>
    /// Adds an asynchronous action to be executed foreach item returned by the items function.
    /// This method will be executed the first time the task is executed.
    /// </summary>
    /// <typeparam name="TData">The type of the data context.</typeparam>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="builder">The task builder.</param>
    /// <param name="itemsFunc">The items.</param>
    /// <param name="action">The action.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Pack")
    ///     .DoesForEach&lt;Foo, string&gt;(data => data.Frameworks, async (data, tfm) =>
    /// {
    ///     await System.Threading.Tasks.Task.Delay(10);
    ///     Information("Packing {0} for {1}", tfm, data.Place);
    /// });
    /// </code>
    /// </example>
    public static CakeTaskBuilder DoesForEach<TData, TItem>(this CakeTaskBuilder builder, Func<TData, IEnumerable<TItem>> itemsFunc, Func<TData, TItem, Task> action) where TData : class
    {
        ArgumentNullException.ThrowIfNull(action);

        return DoesForEach(builder, context => itemsFunc(context.Data.Get<TData>()), (item, context) => action(context.Data.Get<TData>(), item));
    }

    /// <summary>
    /// Adds an asynchronous action to be executed foreach item returned by the items function.
    /// This method will be executed the first time the task is executed.
    /// </summary>
    /// <typeparam name="TData">The type of the data context.</typeparam>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="builder">The task builder.</param>
    /// <param name="itemsFunc">The items.</param>
    /// <param name="action">The action.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Pack")
    ///     .DoesForEach&lt;Foo, string&gt;(data => data.Frameworks, async (tfm, context) =>
    /// {
    ///     await System.Threading.Tasks.Task.Delay(10);
    ///     context.Log.Information("Packing {0}", tfm);
    /// });
    /// </code>
    /// </example>
    public static CakeTaskBuilder DoesForEach<TData, TItem>(this CakeTaskBuilder builder, Func<TData, IEnumerable<TItem>> itemsFunc, Func<TItem, ICakeContext, Task> action) where TData : class
    {
        ArgumentNullException.ThrowIfNull(action);

        return DoesForEach(builder, context => itemsFunc(context.Data.Get<TData>()), action);
    }

    /// <summary>
    /// Adds an asynchronous action to be executed foreach item returned by the items function.
    /// This method will be executed the first time the task is executed.
    /// </summary>
    /// <typeparam name="TData">The type of the data context.</typeparam>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="builder">The task builder.</param>
    /// <param name="itemsFunc">The items.</param>
    /// <param name="action">The action.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Pack")
    ///     .DoesForEach&lt;Foo, string&gt;(data => data.Frameworks, async tfm =>
    /// {
    ///     await System.Threading.Tasks.Task.Delay(10);
    ///     Information("Packing {0}", tfm);
    /// });
    /// </code>
    /// </example>
    public static CakeTaskBuilder DoesForEach<TData, TItem>(this CakeTaskBuilder builder, Func<TData, IEnumerable<TItem>> itemsFunc, Func<TItem, Task> action) where TData : class
    {
        ArgumentNullException.ThrowIfNull(action);

        return DoesForEach(builder, context => itemsFunc(context.Data.Get<TData>()), action);
    }

    /// <summary>
    /// Adds an asynchronous action to be executed foreach item returned by the items function.
    /// This method will be executed the first time the task is executed.
    /// </summary>
    /// <typeparam name="TData">The type of the data context.</typeparam>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="builder">The task builder.</param>
    /// <param name="itemsFunc">The items.</param>
    /// <param name="action">The action.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Pack")
    ///     .DoesForEach&lt;Foo, string&gt;((data, context) => data.Frameworks, async (data, tfm, context) =>
    /// {
    ///     await System.Threading.Tasks.Task.Delay(10);
    ///     context.Log.Information("Packing {0} for {1}", tfm, data.Place);
    /// });
    /// </code>
    /// </example>
    public static CakeTaskBuilder DoesForEach<TData, TItem>(this CakeTaskBuilder builder, Func<TData, ICakeContext, IEnumerable<TItem>> itemsFunc, Func<TData, TItem, ICakeContext, Task> action) where TData : class
    {
        ArgumentNullException.ThrowIfNull(action);

        return DoesForEach(builder, context => itemsFunc(context.Data.Get<TData>(), context), (item, context) => action(context.Data.Get<TData>(), item, context));
    }

    /// <summary>
    /// Adds an asynchronous action to be executed foreach item returned by the items function.
    /// This method will be executed the first time the task is executed.
    /// </summary>
    /// <typeparam name="TData">The type of the data context.</typeparam>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="builder">The task builder.</param>
    /// <param name="itemsFunc">The items.</param>
    /// <param name="action">The action.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Pack")
    ///     .DoesForEach&lt;Foo, string&gt;((data, context) => data.Frameworks, async (data, tfm) =>
    /// {
    ///     await System.Threading.Tasks.Task.Delay(10);
    ///     Information("Packing {0} for {1}", tfm, data.Place);
    /// });
    /// </code>
    /// </example>
    public static CakeTaskBuilder DoesForEach<TData, TItem>(this CakeTaskBuilder builder, Func<TData, ICakeContext, IEnumerable<TItem>> itemsFunc, Func<TData, TItem, Task> action) where TData : class
    {
        ArgumentNullException.ThrowIfNull(action);

        return DoesForEach(builder, context => itemsFunc(context.Data.Get<TData>(), context), (item, context) => action(context.Data.Get<TData>(), item));
    }

    /// <summary>
    /// Adds an asynchronous action to be executed foreach item returned by the items function.
    /// This method will be executed the first time the task is executed.
    /// </summary>
    /// <typeparam name="TData">The type of the data context.</typeparam>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="builder">The task builder.</param>
    /// <param name="itemsFunc">The items.</param>
    /// <param name="action">The action.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Pack")
    ///     .DoesForEach&lt;Foo, string&gt;((data, context) => data.Frameworks, async (tfm, context) =>
    /// {
    ///     await System.Threading.Tasks.Task.Delay(10);
    ///     context.Log.Information("Packing {0}", tfm);
    /// });
    /// </code>
    /// </example>
    public static CakeTaskBuilder DoesForEach<TData, TItem>(this CakeTaskBuilder builder, Func<TData, ICakeContext, IEnumerable<TItem>> itemsFunc, Func<TItem, ICakeContext, Task> action) where TData : class
    {
        ArgumentNullException.ThrowIfNull(action);

        return DoesForEach(builder, context => itemsFunc(context.Data.Get<TData>(), context), action);
    }

    /// <summary>
    /// Adds an asynchronous action to be executed foreach item returned by the items function.
    /// This method will be executed the first time the task is executed.
    /// </summary>
    /// <typeparam name="TData">The type of the data context.</typeparam>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="builder">The task builder.</param>
    /// <param name="itemsFunc">The items.</param>
    /// <param name="action">The action.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Pack")
    ///     .DoesForEach&lt;Foo, string&gt;((data, context) => data.Frameworks, async tfm =>
    /// {
    ///     await System.Threading.Tasks.Task.Delay(10);
    ///     Information("Packing {0}", tfm);
    /// });
    /// </code>
    /// </example>
    public static CakeTaskBuilder DoesForEach<TData, TItem>(this CakeTaskBuilder builder, Func<TData, ICakeContext, IEnumerable<TItem>> itemsFunc, Func<TItem, Task> action) where TData : class
    {
        ArgumentNullException.ThrowIfNull(action);

        return DoesForEach(builder, context => itemsFunc(context.Data.Get<TData>(), context), (item, _) => action(item));
    }

    /// <summary>
    /// Adds an asynchronous action to be executed foreach item returned by the items function.
    /// This method will be executed the first time the task is executed.
    /// </summary>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="builder">The task builder.</param>
    /// <param name="itemsFunc">The items.</param>
    /// <param name="action">The action.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Pack")
    ///     .DoesForEach(context => new[] { "net10.0" }, async (tfm, context) =>
    /// {
    ///     await System.Threading.Tasks.Task.Delay(10);
    ///     context.Log.Information("Packing {0}", tfm);
    /// });
    /// </code>
    /// </example>
    public static CakeTaskBuilder DoesForEach<TItem>(this CakeTaskBuilder builder, Func<ICakeContext, IEnumerable<TItem>> itemsFunc, Func<TItem, ICakeContext, Task> action)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(action);

        builder.Target.AddDelayedAction(delayedContext =>
        {
            foreach (var item in itemsFunc(delayedContext))
            {
                builder.Target.AddAction(context => action(item, context));
            }
        });

        return builder;
    }

    /// <summary>
    /// Adds an action to be executed foreach item in the asynchronous list.
    /// This method will be executed the first time the task is executed.
    /// </summary>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="builder">The task builder.</param>
    /// <param name="items">The items.</param>
    /// <param name="action">The action.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Pack")
    ///     .DoesForEach(GetFrameworksAsync(), tfm =>
    /// {
    ///     Information("Packing {0}", tfm);
    /// });
    /// </code>
    /// </example>
    public static CakeTaskBuilder DoesForEach<TItem>(this CakeTaskBuilder builder, IAsyncEnumerable<TItem> items, Action<TItem> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        return DoesForEach(builder, _ => items, (item, _) =>
        {
            action(item);
            return Task.CompletedTask;
        });
    }

    /// <summary>
    /// Adds an asynchronous action to be executed foreach item in the asynchronous list.
    /// This method will be executed the first time the task is executed.
    /// </summary>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="builder">The task builder.</param>
    /// <param name="items">The items.</param>
    /// <param name="action">The action.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Pack")
    ///     .DoesForEach(GetFrameworksAsync(), async tfm =>
    /// {
    ///     await System.Threading.Tasks.Task.Delay(10);
    ///     Information("Packing {0}", tfm);
    /// });
    /// </code>
    /// </example>
    public static CakeTaskBuilder DoesForEach<TItem>(this CakeTaskBuilder builder, IAsyncEnumerable<TItem> items, Func<TItem, Task> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        return DoesForEach(builder, _ => items, (item, _) => action(item));
    }

    /// <summary>
    /// Adds an action to be executed foreach item in the asynchronous list.
    /// This method will be executed the first time the task is executed.
    /// </summary>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="builder">The task builder.</param>
    /// <param name="items">The items.</param>
    /// <param name="action">The action.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Pack")
    ///     .DoesForEach(GetFrameworksAsync(), (tfm, context) =>
    /// {
    ///     context.Log.Information("Packing {0}", tfm);
    /// });
    /// </code>
    /// </example>
    public static CakeTaskBuilder DoesForEach<TItem>(this CakeTaskBuilder builder, IAsyncEnumerable<TItem> items, Action<TItem, ICakeContext> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        return DoesForEach(builder, _ => items, (item, context) =>
        {
            action(item, context);
            return Task.CompletedTask;
        });
    }

    /// <summary>
    /// Adds an asynchronous action to be executed foreach item in the asynchronous list.
    /// This method will be executed the first time the task is executed.
    /// </summary>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="builder">The task builder.</param>
    /// <param name="items">The items.</param>
    /// <param name="action">The action.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Pack")
    ///     .DoesForEach(GetFrameworksAsync(), async (tfm, context) =>
    /// {
    ///     await System.Threading.Tasks.Task.Delay(10);
    ///     context.Log.Information("Packing {0}", tfm);
    /// });
    /// </code>
    /// </example>
    public static CakeTaskBuilder DoesForEach<TItem>(this CakeTaskBuilder builder, IAsyncEnumerable<TItem> items, Func<TItem, ICakeContext, Task> action)
    {
        return DoesForEach(builder, _ => items, action);
    }

    /// <summary>
    /// Adds an action to be executed foreach item returned by the asynchronous items function.
    /// This method will be executed the first time the task is executed.
    /// </summary>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="builder">The task builder.</param>
    /// <param name="itemsFunc">The items.</param>
    /// <param name="action">The action.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Pack")
    ///     .DoesForEach(() => GetFrameworksAsync(), tfm =>
    /// {
    ///     Information("Packing {0}", tfm);
    /// });
    /// </code>
    /// </example>
    public static CakeTaskBuilder DoesForEach<TItem>(this CakeTaskBuilder builder, Func<IAsyncEnumerable<TItem>> itemsFunc, Action<TItem> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        return DoesForEach(builder, _ => itemsFunc(), (item, _) =>
        {
            action(item);
            return Task.CompletedTask;
        });
    }

    /// <summary>
    /// Adds an asynchronous action to be executed foreach item returned by the asynchronous items function.
    /// This method will be executed the first time the task is executed.
    /// </summary>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="builder">The task builder.</param>
    /// <param name="itemsFunc">The items.</param>
    /// <param name="action">The action.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Pack")
    ///     .DoesForEach(() => GetFrameworksAsync(), async tfm =>
    /// {
    ///     await System.Threading.Tasks.Task.Delay(10);
    ///     Information("Packing {0}", tfm);
    /// });
    /// </code>
    /// </example>
    public static CakeTaskBuilder DoesForEach<TItem>(this CakeTaskBuilder builder, Func<IAsyncEnumerable<TItem>> itemsFunc, Func<TItem, Task> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        return DoesForEach(builder, _ => itemsFunc(), (item, _) => action(item));
    }

    /// <summary>
    /// Adds an action to be executed foreach item returned by the asynchronous items function.
    /// This method will be executed the first time the task is executed.
    /// </summary>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="builder">The task builder.</param>
    /// <param name="itemsFunc">The items.</param>
    /// <param name="action">The action.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Pack")
    ///     .DoesForEach(context => GetFrameworksAsync(), tfm =>
    /// {
    ///     Information("Packing {0}", tfm);
    /// });
    /// </code>
    /// </example>
    public static CakeTaskBuilder DoesForEach<TItem>(this CakeTaskBuilder builder, Func<ICakeContext, IAsyncEnumerable<TItem>> itemsFunc, Action<TItem> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        return DoesForEach(builder, itemsFunc, (item, _) =>
        {
            action(item);
            return Task.CompletedTask;
        });
    }

    /// <summary>
    /// Adds an asynchronous action to be executed foreach item returned by the asynchronous items function.
    /// This method will be executed the first time the task is executed.
    /// </summary>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="builder">The task builder.</param>
    /// <param name="itemsFunc">The items.</param>
    /// <param name="action">The action.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Pack")
    ///     .DoesForEach(context => GetFrameworksAsync(), async tfm =>
    /// {
    ///     await System.Threading.Tasks.Task.Delay(10);
    ///     Information("Packing {0}", tfm);
    /// });
    /// </code>
    /// </example>
    public static CakeTaskBuilder DoesForEach<TItem>(this CakeTaskBuilder builder, Func<ICakeContext, IAsyncEnumerable<TItem>> itemsFunc, Func<TItem, Task> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        return DoesForEach(builder, itemsFunc, (item, _) => action(item));
    }

    /// <summary>
    /// Adds an action to be executed foreach item returned by the asynchronous items function.
    /// This method will be executed the first time the task is executed.
    /// </summary>
    /// <typeparam name="TData">The type of the data context.</typeparam>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="builder">The task builder.</param>
    /// <param name="itemsFunc">The items.</param>
    /// <param name="action">The action.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Pack")
    ///     .DoesForEach&lt;Foo, string&gt;(data => data.FrameworksAsync, (data, tfm) =>
    /// {
    ///     Information("Packing {0} for {1}", tfm, data.Place);
    /// });
    /// </code>
    /// </example>
    public static CakeTaskBuilder DoesForEach<TData, TItem>(this CakeTaskBuilder builder, Func<TData, IAsyncEnumerable<TItem>> itemsFunc, Action<TData, TItem> action) where TData : class
    {
        ArgumentNullException.ThrowIfNull(action);

        return DoesForEach(builder, context => itemsFunc(context.Data.Get<TData>()), (item, context) =>
        {
            action(context.Data.Get<TData>(), item);
            return Task.CompletedTask;
        });
    }

    /// <summary>
    /// Adds an asynchronous action to be executed foreach item returned by the asynchronous items function.
    /// This method will be executed the first time the task is executed.
    /// </summary>
    /// <typeparam name="TData">The type of the data context.</typeparam>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="builder">The task builder.</param>
    /// <param name="itemsFunc">The items.</param>
    /// <param name="action">The action.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Pack")
    ///     .DoesForEach&lt;Foo, string&gt;(data => data.FrameworksAsync, async (data, tfm) =>
    /// {
    ///     await System.Threading.Tasks.Task.Delay(10);
    ///     Information("Packing {0} for {1}", tfm, data.Place);
    /// });
    /// </code>
    /// </example>
    public static CakeTaskBuilder DoesForEach<TData, TItem>(this CakeTaskBuilder builder, Func<TData, IAsyncEnumerable<TItem>> itemsFunc, Func<TData, TItem, Task> action) where TData : class
    {
        ArgumentNullException.ThrowIfNull(action);

        return DoesForEach(builder, context => itemsFunc(context.Data.Get<TData>()), (item, context) => action(context.Data.Get<TData>(), item));
    }

    /// <summary>
    /// Adds an action to be executed foreach item returned by the asynchronous items function.
    /// This method will be executed the first time the task is executed.
    /// </summary>
    /// <typeparam name="TData">The type of the data context.</typeparam>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="builder">The task builder.</param>
    /// <param name="itemsFunc">The items.</param>
    /// <param name="action">The action.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Pack")
    ///     .DoesForEach&lt;Foo, string&gt;(data => data.FrameworksAsync, (data, tfm, context) =>
    /// {
    ///     context.Log.Information("Packing {0} for {1}", tfm, data.Place);
    /// });
    /// </code>
    /// </example>
    public static CakeTaskBuilder DoesForEach<TData, TItem>(this CakeTaskBuilder builder, Func<TData, IAsyncEnumerable<TItem>> itemsFunc, Action<TData, TItem, ICakeContext> action) where TData : class
    {
        ArgumentNullException.ThrowIfNull(action);

        return DoesForEach(builder, context => itemsFunc(context.Data.Get<TData>()), (item, context) =>
        {
            action(context.Data.Get<TData>(), item, context);
            return Task.CompletedTask;
        });
    }

    /// <summary>
    /// Adds an asynchronous action to be executed foreach item returned by the asynchronous items function.
    /// This method will be executed the first time the task is executed.
    /// </summary>
    /// <typeparam name="TData">The type of the data context.</typeparam>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="builder">The task builder.</param>
    /// <param name="itemsFunc">The items.</param>
    /// <param name="action">The action.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Pack")
    ///     .DoesForEach&lt;Foo, string&gt;(data => data.FrameworksAsync, async (data, tfm, context) =>
    /// {
    ///     await System.Threading.Tasks.Task.Delay(10);
    ///     context.Log.Information("Packing {0} for {1}", tfm, data.Place);
    /// });
    /// </code>
    /// </example>
    public static CakeTaskBuilder DoesForEach<TData, TItem>(this CakeTaskBuilder builder, Func<TData, IAsyncEnumerable<TItem>> itemsFunc, Func<TData, TItem, ICakeContext, Task> action) where TData : class
    {
        ArgumentNullException.ThrowIfNull(action);

        return DoesForEach(builder, context => itemsFunc(context.Data.Get<TData>()), (item, context) => action(context.Data.Get<TData>(), item, context));
    }

    /// <summary>
    /// Adds an action to be executed foreach item returned by the asynchronous items function.
    /// This method will be executed the first time the task is executed.
    /// </summary>
    /// <typeparam name="TData">The type of the data context.</typeparam>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="builder">The task builder.</param>
    /// <param name="itemsFunc">The items.</param>
    /// <param name="action">The action.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Pack")
    ///     .DoesForEach&lt;Foo, string&gt;((data, context) => data.FrameworksAsync, (data, tfm, context) =>
    /// {
    ///     context.Log.Information("Packing {0} for {1}", tfm, data.Place);
    /// });
    /// </code>
    /// </example>
    public static CakeTaskBuilder DoesForEach<TData, TItem>(this CakeTaskBuilder builder, Func<TData, ICakeContext, IAsyncEnumerable<TItem>> itemsFunc, Action<TData, TItem, ICakeContext> action) where TData : class
    {
        ArgumentNullException.ThrowIfNull(action);

        return DoesForEach(builder, context => itemsFunc(context.Data.Get<TData>(), context), (item, context) =>
        {
            action(context.Data.Get<TData>(), item, context);
            return Task.CompletedTask;
        });
    }

    /// <summary>
    /// Adds an asynchronous action to be executed foreach item returned by the asynchronous items function.
    /// This method will be executed the first time the task is executed.
    /// </summary>
    /// <typeparam name="TData">The type of the data context.</typeparam>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="builder">The task builder.</param>
    /// <param name="itemsFunc">The items.</param>
    /// <param name="action">The action.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Pack")
    ///     .DoesForEach&lt;Foo, string&gt;((data, context) => data.FrameworksAsync, async (data, tfm, context) =>
    /// {
    ///     await System.Threading.Tasks.Task.Delay(10);
    ///     context.Log.Information("Packing {0} for {1}", tfm, data.Place);
    /// });
    /// </code>
    /// </example>
    public static CakeTaskBuilder DoesForEach<TData, TItem>(this CakeTaskBuilder builder, Func<TData, ICakeContext, IAsyncEnumerable<TItem>> itemsFunc, Func<TData, TItem, ICakeContext, Task> action) where TData : class
    {
        ArgumentNullException.ThrowIfNull(action);

        return DoesForEach(builder, context => itemsFunc(context.Data.Get<TData>(), context), (item, context) => action(context.Data.Get<TData>(), item, context));
    }

    /// <summary>
    /// Adds an action to be executed foreach item returned by the asynchronous items function.
    /// This method will be executed the first time the task is executed.
    /// </summary>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="builder">The task builder.</param>
    /// <param name="itemsFunc">The items.</param>
    /// <param name="action">The action.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Pack")
    ///     .DoesForEach(context => GetFrameworksAsync(), (tfm, context) =>
    /// {
    ///     context.Log.Information("Packing {0}", tfm);
    /// });
    /// </code>
    /// </example>
    public static CakeTaskBuilder DoesForEach<TItem>(this CakeTaskBuilder builder, Func<ICakeContext, IAsyncEnumerable<TItem>> itemsFunc, Action<TItem, ICakeContext> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        return DoesForEach(builder, itemsFunc, (item, context) =>
        {
            action(item, context);
            return Task.CompletedTask;
        });
    }

    /// <summary>
    /// Adds an asynchronous action to be executed foreach item returned by the asynchronous items function.
    /// This method will be executed the first time the task is executed.
    /// </summary>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="builder">The task builder.</param>
    /// <param name="itemsFunc">The items.</param>
    /// <param name="action">The action.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Pack")
    ///     .DoesForEach(context => GetFrameworksAsync(), async (tfm, context) =>
    /// {
    ///     await System.Threading.Tasks.Task.Delay(10);
    ///     context.Log.Information("Packing {0}", tfm);
    /// });
    /// </code>
    /// </example>
    public static CakeTaskBuilder DoesForEach<TItem>(this CakeTaskBuilder builder, Func<ICakeContext, IAsyncEnumerable<TItem>> itemsFunc, Func<TItem, ICakeContext, Task> action)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(action);

        builder.Target.AddDelayedAction(async delayedContext =>
        {
            await foreach (var item in itemsFunc(delayedContext))
            {
                builder.Target.AddAction(context => action(item, context));
            }
        });

        return builder;
    }
}
