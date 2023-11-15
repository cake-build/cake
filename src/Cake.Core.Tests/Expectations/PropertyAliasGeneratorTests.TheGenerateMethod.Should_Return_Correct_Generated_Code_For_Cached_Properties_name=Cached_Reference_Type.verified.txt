private System.String _Cached_Reference_Type;
public System.String Cached_Reference_Type
{
    [System.Diagnostics.DebuggerStepThrough]
    get
    {
        if (_Cached_Reference_Type==null)
        {
            _Cached_Reference_Type = Cake.Core.Tests.Data.PropertyAliasGeneratorData.Cached_Reference_Type(Context);
        }
        return _Cached_Reference_Type;
    }
}