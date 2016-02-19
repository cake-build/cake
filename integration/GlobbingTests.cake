#load "./utilities/test.cake"
#load "./utilities/ensure_directory.cake"

//////////////////////////////////////////////////////////////////////
// TEST
//////////////////////////////////////////////////////////////////////

Task("Should_Find_Directories_With_Pattern")
	.Does(() =>
{
	try
	{
		// Given
		Ensure.That_Directory_Exist("./temp/Hello");
		Ensure.That_Directory_Exist("./temp/Goodbye");
		Ensure.That_Directory_Exist("./temp/Hello/World");
		// When
		var result = GetDirectories("./temp/**/World");
		// Then
		Assert.Equal(1, result.Count);
	}
	finally
	{
		// Clean up
		Ensure.That_Directory_Do_Not_Exist("./temp/Hello/World");
		Ensure.That_Directory_Do_Not_Exist("./temp/Hello");
		Ensure.That_Directory_Do_Not_Exist("./temp/World");
		Ensure.That_Directory_Do_Not_Exist("./temp");
	}
});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTests();