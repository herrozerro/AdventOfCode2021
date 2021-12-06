<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

#load "utilities.linq"

async Task Main()
{
	//testing
	Debug.WriteLine("Beginning Tests");
	Debug.Assert(await p1(true) == 5934);
	Debug.Assert(await p2(true) == 26984457539);
	Debug.WriteLine("Tests Complete");

	await p1().Dump();
	await p2().Dump();
}

// You can define other methods, fields, classes and namespaces here
public async Task<int> p1(bool test = false)
{
	var line = await GetStringFromFile("d06", test);

	var fish = line.Split(',').Select(x => int.Parse(x.ToString())).ToList();

	for (int day = 0; day < 80; day++)
	{
		var fishstart = fish.Count();
		for (int i = 0; i < fishstart; i++)
		{
			if (fish[i] == 0)
			{
				fish.Add(8);
				fish[i] = 6;
			}
			else
			{
				fish[i]--;
			}
		}
	}

	return fish.Count();
}

public async Task<long> p2(bool test = false)
{
	var line = await GetStringFromFile("d06", test);

	var fish = line.Split(',').Select(x => int.Parse(x.ToString())).ToList();

	var holdingpens = new Dictionary<int, long>();

	foreach (var f in fish)
	{
		if (holdingpens.ContainsKey(f))
		{
			holdingpens[f]++;
		}
		else
		{
			holdingpens.Add(f, 1);
		}
	}

	for (int day = 0; day < 256; day++)
	{
		//each morning move fish to a lower holding pen
		foreach (var pen in holdingpens.OrderBy(x => x.Key))
		{
			if (holdingpens.ContainsKey(pen.Key - 1))
			{
				holdingpens[pen.Key - 1] = pen.Value;
			}
			else
			{
				holdingpens.Add(pen.Key - 1, pen.Value);
			}
			holdingpens.Remove(pen.Key);
		}

		//move holding pen -1 to 6, and add that many to holding pen 8
		if (holdingpens.ContainsKey(-1))
		{
			if (holdingpens.ContainsKey(6))
			{
				holdingpens[6] += holdingpens[-1];
			}
			else
			{
				holdingpens.Add(6, holdingpens[-1]);
			}
			holdingpens.Add(8, holdingpens[-1]);
			holdingpens.Remove(-1);
		}


	}

	return holdingpens.Sum(h => h.Value).Dump();
}