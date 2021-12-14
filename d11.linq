<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

#load "utilities.linq"

async Task Main()
{
	//testing
	Debug.WriteLine("Beginning Tests");
	Debug.Assert(await p1(true) == 1656);
	Debug.Assert(await p2(true) == 195);
	Debug.WriteLine("Tests Complete");

	await p1().Dump();
	await p2().Dump();
}

// You can define other methods, fields, classes and namespaces here
public async Task<int> p1(bool test = false)
{
	var lines = await GetLinesFromFile("d11", test);

	var map = new Dictionary<(int x, int y), int>();

	for (int i = 0; i < lines.Length; i++)
	{
		for (int j = 0; j < lines[i].Length; j++)
		{
			map.Add((j, i), int.Parse(lines[i][j].ToString()));
		}
	}

	var totalFlashes = 0;
	
	for (int i = 0; i < 100; i++)
	{
		totalFlashes += iterate(map);
	}
	
	return totalFlashes;
}

public async Task<int> p2(bool test = false)
{
	var lines = await GetLinesFromFile("d11", test);

	var map = new Dictionary<(int x, int y), int>();

	for (int i = 0; i < lines.Length; i++)
	{
		for (int j = 0; j < lines[i].Length; j++)
		{
			map.Add((j, i), int.Parse(lines[i][j].ToString()));
		}
	}
	
	var step = 1;
	while (true)
	{
		if (iterate(map) == 100)
		{
			break;
		}
		step++;
	}

	return step;
}

private List<(int x, int y)> AdjectentMatrix()
{
	return new List<(int x, int y)>()
	{
		(-1,-1),
		(0,-1),
		(1,-1),
		(-1,0),
		(1,0),
		(-1,1),
		(0,1),
		(1,1)
	};
}

private int iterate(Dictionary<(int x, int y), int> map)
{
	var matrix = AdjectentMatrix();
	//increase each element by 1
	foreach (var key in map.Keys)
	{
		map[key]++;
	}
	
	var totalFlashes = 0;
	
	while (map.Any(m => m.Value > 9))
	{
		var flashes = map.Where(m => m.Value > 9);
		
		foreach (var flash in flashes)
		{
			foreach (var item in matrix)
			{
				var adjectent = (flash.Key.x + item.x, flash.Key.y + item.y);
				if (map.ContainsKey(adjectent))
				{
					if (map[adjectent] != 0)
					{
						map[adjectent]++;
					}
				}
			}
			
			map[flash.Key] = 0;
		}
	}
	
	
	return map.Count(m => m.Value == 0);
}