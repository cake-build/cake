using System;
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
}