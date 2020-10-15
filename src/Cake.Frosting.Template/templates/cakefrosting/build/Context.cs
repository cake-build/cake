using Cake.Core;
using Cake.Frosting;

public class Context : FrostingContext
{
    public Context(ICakeContext context) 
        : base(context)
    {
    }
}