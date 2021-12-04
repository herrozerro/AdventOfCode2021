<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

#load "utilities.linq"

async Task Main()
{
	//testing
	Debug.WriteLine("Beginning Tests");
	Debug.Assert(await p1(true) == 7);
	Debug.Assert(await p2(true) == 5);
	Debug.WriteLine("Tests Complete");

	await p1().Dump();
	await p2().Dump();
}

public async Task<int> p1(bool test = false)
{
	var lines = await GetLinesFromFile("d01", test);

	var inc = 0;
	var prev = int.Parse(lines.First());

	foreach (var line in lines.Skip(1))
	{
		if (int.Parse(line) > prev)
		{
			inc++;
		}
		prev = int.Parse(line);
	}

	return inc;
}

public async Task<int> p2(bool test = false)
{
	var lines = await GetLinesFromFile("d01", test);

	var inc = 0;
	var prev = int.Parse(lines.First());
	int offset = 0;
	var window = new List<int>();

	prev = lines.Skip(offset).Take(3).Select(x => int.Parse(x)).Sum();

	foreach (var line in lines.Skip(offset))
	{
		if (lines.Skip(offset).Count() < 3)
		{
			break;
		}

		int cur = lines.Skip(offset).Take(3).Select(x => int.Parse(x)).Sum();

		if (cur > prev)
		{
			inc++;
		}
		prev = lines.Skip(offset).Take(3).Select(x => int.Parse(x)).Sum();
		offset++;
	}

	return inc;
}