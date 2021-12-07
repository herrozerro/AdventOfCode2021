<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

#load "utilities.linq"

async Task Main()
{
	//testing
	Debug.WriteLine("Beginning Tests");
	Debug.Assert(await p1(true) == 37);
	Debug.Assert(await p2(true) == 168);
	Debug.WriteLine("Tests Complete");

	await p1().Dump();
	await p2().Dump();
}

// You can define other methods, fields, classes and namespaces here
public async Task<int> p1(bool test = false)
{
	var line = await GetStringFromFile("d07", test);
	
	var crabs = line.Split(',').Select(x => int.Parse(x)).OrderBy(x => x).ToList();
	
	var fuelusage = 0;
	
	foreach (var crab in crabs)
	{
		var fuel = crabs.Sum(c => Math.Abs(c - crab));
		if (fuel < fuelusage || fuelusage == 0)
		{
			fuelusage = fuel;
		}
	}
	
	return fuelusage;
}

public async Task<int> p2(bool test = false)
{
	var line = await GetStringFromFile("d07", test);

	var crabs = line.Split(',').Select(x => int.Parse(x)).OrderBy(x => x).ToList();

	var fuelusage = 0;

	for (int pos = 0; pos < crabs.Max(); pos++)
	{
		var fuelUsed = 0;
		
		foreach (var crab in crabs)
		{
			var steps = Math.Abs(pos - crab);
			for (int i = 0; i < steps; i++)
			{
				fuelUsed += 1 + i;
			}
		}
		
		if (fuelUsed < fuelusage || fuelusage == 0)
		{
			fuelusage = fuelUsed;
		}
	}

	return fuelusage;
}