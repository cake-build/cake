// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Cake.Core;

public static partial class CakeTaskBuilderExtensions
{
    /// <summary>
    /// Creates a dependency between two tasks.
    /// </summary>
    /// <param name="builder">The task builder.</param>
    /// <param name="name">The name of the dependent task.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Default")
    ///     .IsDependentOn("Build");
    /// </code>
    /// </example>
    public static CakeTaskBuilder IsDependentOn(this CakeTaskBuilder builder, string name)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Target.AddDependency(name);
        return builder;
    }

    /// <summary>
    /// Creates dependencies on the specified tasks.
    /// </summary>
    /// <param name="builder">The task builder.</param>
    /// <param name="names">The names of the dependent tasks.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Default")
    ///     .IsDependentOn(["Clean", "Build"]);
    /// </code>
    /// </example>
    public static CakeTaskBuilder IsDependentOn(this CakeTaskBuilder builder, IEnumerable<string> names)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(names);

        foreach (var name in names)
        {
            ArgumentNullException.ThrowIfNull(name);
            builder.IsDependentOn(name);
        }

        return builder;
    }

    /// <summary>
    /// Creates a dependency between two tasks.
    /// </summary>
    /// <param name="builder">The task builder.</param>
    /// <param name="other">The name of the dependent task.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// var build = Task("Build");
    /// Task("Default")
    ///     .IsDependentOn(build);
    /// </code>
    /// </example>
    public static CakeTaskBuilder IsDependentOn(this CakeTaskBuilder builder, CakeTaskBuilder other)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(other);

        builder.Target.AddDependency(other.Target.Name);
        return builder;
    }

    /// <summary>
    /// Creates dependencies on the specified tasks.
    /// </summary>
    /// <param name="builder">The task builder.</param>
    /// <param name="others">The dependent tasks.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// var test1 = Task("Test1");
    /// var test2 = Task("Test2");
    /// Task("Default")
    ///     .IsDependentOn([test1, test2]);
    /// </code>
    /// </example>
    public static CakeTaskBuilder IsDependentOn(this CakeTaskBuilder builder, IEnumerable<CakeTaskBuilder> others)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(others);

        foreach (var other in others)
        {
            builder.IsDependentOn(other);
        }

        return builder;
    }

    /// <summary>
    /// Makes the task a dependency of another task.
    /// </summary>
    /// <param name="builder">The task builder.</param>
    /// <param name="name">The name of the task the current task will be a dependency of.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Clean")
    ///     .IsDependeeOf("Build");
    /// </code>
    /// </example>
    public static CakeTaskBuilder IsDependeeOf(this CakeTaskBuilder builder, string name)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Target.AddDependee(name);
        return builder;
    }

    /// <summary>
    /// Makes the task a dependency of the specified tasks.
    /// </summary>
    /// <param name="builder">The task builder.</param>
    /// <param name="names">The names of the tasks the current task will be a dependency of.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// Task("Leaf")
    ///     .IsDependeeOf(["Default", "CI"]);
    /// </code>
    /// </example>
    public static CakeTaskBuilder IsDependeeOf(this CakeTaskBuilder builder, IEnumerable<string> names)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(names);

        foreach (var name in names)
        {
            ArgumentNullException.ThrowIfNull(name);
            builder.IsDependeeOf(name);
        }

        return builder;
    }

    /// <summary>
    /// Makes the task a dependency of another task.
    /// </summary>
    /// <param name="builder">The task builder.</param>
    /// <param name="other">The name of the dependent task.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// var build = Task("Build");
    /// Task("Clean")
    ///     .IsDependeeOf(build);
    /// </code>
    /// </example>
    public static CakeTaskBuilder IsDependeeOf(this CakeTaskBuilder builder, CakeTaskBuilder other)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(other);

        builder.Target.AddDependee(other.Target.Name);
        return builder;
    }

    /// <summary>
    /// Makes the task a dependency of the specified tasks.
    /// </summary>
    /// <param name="builder">The task builder.</param>
    /// <param name="others">The tasks the current task will be a dependency of.</param>
    /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// var defaultTask = Task("Default");
    /// var ciTask = Task("CI");
    /// Task("Leaf")
    ///     .IsDependeeOf([defaultTask, ciTask]);
    /// </code>
    /// </example>
    public static CakeTaskBuilder IsDependeeOf(this CakeTaskBuilder builder, IEnumerable<CakeTaskBuilder> others)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(others);

        foreach (var other in others)
        {
            builder.IsDependeeOf(other);
        }

        return builder;
    }
}
