#load "./utilities/test.cake"
#load "./utilities/ensure_directory.cake"

//////////////////////////////////////////////////////////////////////
// TEST
//////////////////////////////////////////////////////////////////////

Task("Should_Create_Directory")
	.Description("CreateDirectory")
	.Does(() =>
{
	try
	{
		// Given
		Ensure.That_Directory_Do_Not_Exist("./temp");
		// When
		CreateDirectory("./temp");
		// Then
		Assert.True(DirectoryExists("./temp"));
	}
	finally
	{
		// Clean up
		Ensure.That_Directory_Do_Not_Exist("./temp");
	}
});

Task("Should_Return_True_If_Directory_Exist")
	.Does(() =>
{
	try
	{
		// Given
		Ensure.That_Directory_Exist("./temp");
		// When
		var result = DirectoryExists("./temp");
		// Then
		Assert.True(result);
	}
	finally
	{
		// Clean up
		Ensure.That_Directory_Do_Not_Exist("./temp");
	}
});

Task("Should_Return_False_If_Directory_Do_Not_Exist")
	.Does(() =>
{
	// Given
	Ensure.That_Directory_Do_Not_Exist("./temp");
	// When
	var result = DirectoryExists("./temp");
	// Then
	Assert.False(result);
});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTests();