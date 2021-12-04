<Query Kind="Program">
  <NuGetReference>NUnit</NuGetReference>
  <Namespace>NUnit</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

#load "utilities.linq"

async Task Main()
{
	//testing
	Debug.WriteLine("Beginning Tests");
	Debug.Assert(await p1(true) == 150);
	Debug.Assert(await p2(true) == 900);
	Debug.WriteLine("Tests Complete");
	
	await p1().Dump();
	await p2().Dump();
}

// You can define other methods, fields, classes and namespaces here
public async Task<int> p1(bool test = false)
{
	var lines = await GetLinesFromFile("d02", test);

	var depth = 0;
	var horz = 0;

	foreach (var line in lines)
	{
		switch (line)
		{
			case string s when s.StartsWith("forward"):
				horz += int.Parse(line.Split(' ').Last());
				break;
			case string s when s.StartsWith("down"):
				depth += int.Parse(line.Split(' ').Last());
				break;
			case string s when s.StartsWith("up"):
				depth -= int.Parse(line.Split(' ').Last());
				break;
		}
	}

	return (horz * depth);
}

public async Task<int> p2(bool test = false)
{
	var lines = await GetLinesFromFile("d02", test);

	var depth = 0;
	var horz = 0;
	var aim = 0;

	foreach (var line in lines)
	{
		switch (line)
		{
			case string s when s.StartsWith("forward"):
				horz += int.Parse(line.Split(' ').Last());
				depth += aim * int.Parse(line.Split(' ').Last());
				break;
			case string s when s.StartsWith("down"):
				aim += int.Parse(line.Split(' ').Last());
				break;
			case string s when s.StartsWith("up"):
				aim -= int.Parse(line.Split(' ').Last());
				break;
		}
	}

	return (horz * depth);
}