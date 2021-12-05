<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

#load "utilities.linq"

async Task Main()
{
	//testing
	Debug.WriteLine("Beginning Tests");
	Debug.Assert(await p1(true) == 5);
	Debug.Assert(await p2(true) == 12);
	Debug.WriteLine("Tests Complete");

	await p1().Dump();
	await p2().Dump();
}

// You can define other methods, fields, classes and namespaces here
public async Task<int> p1(bool test = false)
{
	var lines = await GetLinesFromFile("d05", test);

	var grid = new Dictionary<string, int>();

	foreach (var line in lines)
	{
		var coords = line.Replace(" -> ", ",").Split(',').Select(x => int.Parse(x)).ToList();

		//verticle line
		if (coords[0] == coords[2])
		{

			switch (Math.Sign(coords[1] - coords[3]))
			{
				case -1: //Line going ->
					for (int i = coords[1]; i <= coords[3]; i++)
					{
						if (grid.ContainsKey($"{i},{coords[0]}"))
						{
							grid[$"{i},{coords[0]}"]++;
						}
						else
						{
							grid.Add($"{i},{coords[0]}", 1);
						}

					}
					break;
				case 1:  //Line going <-
					for (int i = coords[1]; i >= coords[3]; i--)
					{
						if (grid.ContainsKey($"{i},{coords[0]}"))
						{
							grid[$"{i},{coords[0]}"]++;
						}
						else
						{
							grid.Add($"{i},{coords[0]}", 1);
						}
					}
					break;
			}

		}
		//horizontal line
		else if (coords[1] == coords[3])
		{
			switch (Math.Sign(coords[0] - coords[2]))
			{
				case -1: //Line going ^
					for (int i = coords[0]; i <= coords[2]; i++)
					{
						if (grid.ContainsKey($"{coords[1]},{i}"))
						{
							grid[$"{coords[1]},{i}"]++;
						}
						else
						{
							grid.Add($"{coords[1]},{i}", 1);
						}
					}
					break;
				case 1:  //Line going v
					for (int i = coords[0]; i >= coords[2]; i--)
					{
						if (grid.ContainsKey($"{coords[1]},{i}"))
						{
							grid[$"{coords[1]},{i}"]++;
						}
						else
						{
							grid.Add($"{coords[1]},{i}", 1);
						}
					}
					break;
			}
		}
	}


	int xmax = grid.Max(g => g.Key.Split(',').Select(k => int.Parse(k.ToString())).First());
	int ymax = grid.Max(g => g.Key.Split(',').Select(k => int.Parse(k.ToString())).Last());

	var rendergrid = new int[xmax + 1, ymax + 1];

	foreach (var cell in grid)
	{
		rendergrid[int.Parse(cell.Key.Split(',').First()), int.Parse(cell.Key.Split(',').Last())] = cell.Value;
	}

	//rendergrid.Dump();
	return grid.Where(g => g.Value > 1).Count();
}

public async Task<int> p2(bool test = false)
{
	var lines = await GetLinesFromFile("d05", test);

	var grid = new Dictionary<string, int>();

	foreach (var line in lines)
	{
		var coords = line.Replace(" -> ", ",").Split(',').Select(x => int.Parse(x)).ToList();

		//verticle line
		if (coords[0] == coords[2])
		{

			switch (Math.Sign(coords[1] - coords[3]))
			{
				case -1: //Line going ^
					for (int i = coords[1]; i <= coords[3]; i++)
					{
						if (grid.ContainsKey($"{i},{coords[0]}"))
						{
							grid[$"{i},{coords[0]}"]++;
						}
						else
						{
							grid.Add($"{i},{coords[0]}", 1);
						}

					}
					break;
				case 1:  //Line going v
					for (int i = coords[1]; i >= coords[3]; i--)
					{
						if (grid.ContainsKey($"{i},{coords[0]}"))
						{
							grid[$"{i},{coords[0]}"]++;
						}
						else
						{
							grid.Add($"{i},{coords[0]}", 1);
						}
					}
					break;
			}

		}
		//horizontal line
		else if (coords[1] == coords[3])
		{
			switch (Math.Sign(coords[0] - coords[2]))
			{
				case -1: //Line going ->
					for (int i = coords[0]; i <= coords[2]; i++)
					{
						if (grid.ContainsKey($"{coords[1]},{i}"))
						{
							grid[$"{coords[1]},{i}"]++;
						}
						else
						{
							grid.Add($"{coords[1]},{i}", 1);
						}
					}
					break;
				case 1:  //Line going <-
					for (int i = coords[0]; i >= coords[2]; i--)
					{
						if (grid.ContainsKey($"{coords[1]},{i}"))
						{
							grid[$"{coords[1]},{i}"]++;
						}
						else
						{
							grid.Add($"{coords[1]},{i}", 1);
						}
					}
					break;
			}
		}
		//diagonal lines
		else
		{
			var x1 = coords[0];
			var y1 = coords[1];
			var x2 = coords[2];
			var y2 = coords[3];

			//flip p1 and p2 if we need to so we always draw in the same direction ->
			if (x1 > x2)
			{
				x1 = coords[2];
				y1 = coords[3];
				x2 = coords[0];
				y2 = coords[1];
			}

			//positive down, negative up
			var slope = ((y1 - y2) / (x1 - x2));
			var newy = y1;
			for (int i = x1; i <= x2; i++)
			{
				var newCoord = $"{newy},{i}";

				if (grid.ContainsKey(newCoord))
				{
					grid[newCoord]++;
				}
				else
				{
					grid.Add(newCoord, 1);
				}
				newy += slope;
			}
		}
	}

	int xmax = grid.Max(g => g.Key.Split(',').Select(k => int.Parse(k.ToString())).First());
	int ymax = grid.Max(g => g.Key.Split(',').Select(k => int.Parse(k.ToString())).Last());

	var rendergrid = new int[xmax + 1, ymax + 1];

	foreach (var cell in grid)
	{
		var x = int.Parse(cell.Key.Split(',').First());
		var y = int.Parse(cell.Key.Split(',').Last());
		rendergrid[x, y] = cell.Value;
	}
	
	rendergrid.Dump();

	return grid.Where(g => g.Value > 1).Count().Dump();

}

