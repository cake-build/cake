private EnsureImplementation _ensure;

public EnsureImplementation Ensure
{
	get 
	{ 
		if(_ensure == null)
		{
			_ensure = new EnsureImplementation(Context);
		}
		return _ensure;
	}	
}

public partial class EnsureImplementation
{
	private readonly ICakeContext _context;

	public EnsureImplementation(ICakeContext context)
	{
		_context = context;
	}
}