<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
	
}

// You can define other methods, fields, classes and namespaces here
public async Task<string[]> GetLinesFromFile(string filename, bool test = false)
{
	if (test)
	{
		filename += "t";
	}
	var lines = await System.IO.File.ReadAllLinesAsync(Path.GetDirectoryName(Util.CurrentQueryPath) + "\\Data\\" + filename + ".txt");

	return lines;
}

public async Task<string> GetStringFromFile(string filename, bool test = false)
{
	if (test)
	{
		filename += "t";
	}
	
	var text = await System.IO.File.ReadAllTextAsync(Path.GetDirectoryName(Util.CurrentQueryPath) + "\\Data\\" + filename + ".txt");

	return text;
}