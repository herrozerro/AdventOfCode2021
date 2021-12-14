<Query Kind="Program">
  <NuGetReference>morelinq</NuGetReference>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

#load "utilities.linq"
//credit to https://github.com/viceroypenguin/ for the algorithm


async Task Main()
{
	//testing
	Debug.WriteLine("Beginning Tests");
	Debug.Assert(await p1(true) == 1588);
	Debug.Assert(await p2(true) == 2188189693529);
	Debug.WriteLine("Tests Complete");

	await p1().Dump();
	await p2().Dump();
}

// You can define other methods, fields, classes and namespaces here
public async Task<int> p1(bool test = false)
{
	var lines = await GetLinesFromFile("d14", test);

	var polymer = lines[0];
	var insertionRules = new Dictionary<string, string>();


	foreach (var line in lines.Skip(2))
	{
		var rule = line.Split(" -> ");
		insertionRules[rule[0]] = rule[1];
	}

	for (int i = 0; i < 10; i++)
	{
		var newPolymer = "";
		for (int j = 0; j < polymer.Length - 1; j++)
		{
			newPolymer += polymer.Substring(j, 1).Insert(1, insertionRules[polymer.Substring(j, 2)]);
		}
		polymer = newPolymer + polymer.Last();
	}

	var chemCount = polymer.GroupBy(p => p).Select(p => new { p.Key, count = p.Count() });

	return chemCount.Max(c => c.count) - chemCount.Min(c => c.count);
}

public async Task<long> p2(bool test = false)
{
	var lines = await GetLinesFromFile("d14", test);

	var pairs = new List<string>();

	for (int i = 0; i < lines[0].Length -1; i++)
	{
		pairs.Add(lines[0].Substring(i,2));
	}

	//just need to track pairs, first letter of each pair is important, as second letter is just the first letter in another pair.
	var polymer = pairs
		.Select(x => (key: string.Join("", x), cnt: 1L))
		.ToList();
	
	// get each instruction and waht it creates.  EG. CH -> B creates CB and BH
	var instructions = lines
		.Skip(2)
		.Select(x => x.Split(" -> "))
		.ToDictionary(
			x => x[0],
			x => new[]
			{
					string.Join("", x[0][0], x[1][0]),
					string.Join("", x[1][0], x[0][1]),
			});

	for (int i = 0; i < 40; i++)
		polymer = insert(polymer, instructions);

	polymer.Add((lines[0].Last().ToString(),1));

	var elements = polymer
			.GroupBy(x => x.key[0], (c, g) => (c, cnt: g.Sum(x => x.cnt)))
			.OrderBy(x => x.cnt)
			.ToList();

	return (elements[^1].cnt - elements[0].cnt);
}

private List<(string key, long cnt)> insert(List<(string key, long cnt)> polymer, Dictionary<string,string[]> instructions)
{
	return polymer.SelectMany(x => instructions[x.key]
					.Select(y => (key: y, x.cnt)))
					.GroupBy(x => x.key, (key, g) => (key, g.Sum(x => x.cnt)))
					.ToList();
}
