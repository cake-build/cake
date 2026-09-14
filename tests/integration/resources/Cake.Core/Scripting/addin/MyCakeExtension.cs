using System;
using System.Collections.Generic;
using System.Dynamic;
using Cake.Core;
using Cake.Core.Annotations;

public static class MyCakeExtension
{
    [CakeMethodAlias]
    public static int GetMagicNumber(this ICakeContext context, bool value)
    {
        return value ? int.MinValue : int.MaxValue;
    }

    [CakeMethodAlias]
    public static int GetMagicNumberOrDefault(this ICakeContext context, bool value, Func<int> defaultValueProvider = null)
    {
        if (value)
        {
            return int.MinValue;
        }

        return defaultValueProvider == null ? int.MaxValue : defaultValueProvider();
    }

    [CakePropertyAlias]
    public static int TheAnswerToLife(this ICakeContext context)
    {
        return 42;
    }

    [CakePropertyAlias(Cache = true)]
    public static dynamic TheDynamicAnswerToLife(this ICakeContext context)
    {
        dynamic value =  new ExpandoObject();
        value.TheAnswerToLife = context.TheAnswerToLife();
        return value;
    }

    [CakeMethodAlias]
    public static dynamic GetDynamicMagicNumber(this ICakeContext context, bool value)
    {
        dynamic result =  new ExpandoObject();
        result.MagicNumber = context.GetMagicNumber(value);
        return result;
    }

#nullable enable
    [CakeMethodAlias]
    public static string GetNullableLabel(this ICakeContext context, string? value)
    {
        return value ?? "none";
    }

    [CakePropertyAlias]
    public static string? TheNullableAnswerToLife(this ICakeContext context)
    {
        return "42";
    }

    [CakeMethodAlias]
    public static int CountNullableLabels(this ICakeContext context, IList<string?> values)
    {
        var count = 0;
        foreach (var value in values)
        {
            if (value != null)
            {
                count++;
            }
        }

        return count;
    }

    [CakeMethodAlias]
    public static T GetNotNullValue<T>(this ICakeContext context, T value)
        where T : notnull
    {
        return value;
    }

    [CakeMethodAlias]
    public static T GetUnconstrainedValue<T>(this ICakeContext context, T value)
    {
        return value;
    }

    [CakeMethodAlias]
    public static int CountUnconstrainedValues<T>(this ICakeContext context, IList<T> values)
    {
        return values.Count;
    }

    [CakeMethodAlias]
    public static T CreateNotNullValue<T>(this ICakeContext context)
        where T : notnull, new()
    {
        return new T();
    }
#nullable restore
}