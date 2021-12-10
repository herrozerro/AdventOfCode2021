<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

#load "utilities.linq"

async Task Main()
{
	//testing
	Debug.WriteLine("Beginning Tests");
	Debug.Assert(await p1(true) == 15);
	Debug.Assert(await p2(true) == 1134);
	Debug.WriteLine("Tests Complete");

	await p1().Dump();
	await p2().Dump();
}

// You can define other methods, fields, classes and namespaces here
public async Task<int> p1(bool test = false)
{
	var lines = await GetLinesFromFile("d09", test);

	var grid = new int[lines.Length, lines[0].Length];

	for (int i = 0; i < lines.Length; i++)
	{
		for (int j = 0; j < lines[i].Length; j++)
		{
			var row = lines[i];
			grid[i, j] = int.Parse(row[j].ToString());
		}
	}

	//grid.Dump();

	var lowPoints = new List<int>();
	var neighbors = new List<int>();
	for (int y = 0; y < grid.GetLength(0); y++)
	{
		for (int x = 0; x < grid.GetLength(1); x++)
		{
			neighbors.Clear();
			var point = grid[y, x];

			//check up
			if (y - 1 >= 0)
			{
				neighbors.Add(grid[y - 1, x]);
			}

			//check down
			if (y + 1 < grid.GetLength(0))
			{
				neighbors.Add(grid[y + 1, x]);
			}

			//check left
			if (x - 1 >= 0)
			{
				neighbors.Add(grid[y, x - 1]);
			}

			//check right
			if (x + 1 < grid.GetLength(1))
			{
				neighbors.Add(grid[y, x + 1]);
			}

			if (neighbors.All(n => n > point))
			{
				//Console.WriteLine($"{x},{y}: {point} is low");
				lowPoints.Add(point);
			}

		}
	}

	var threat = lowPoints.Sum(p => p + 1);
	return threat;
}

public async Task<int> p2(bool test = false)
{
	
	
	var lines = await GetLinesFromFile("d09", test);
	
	var prevgrid = new int[lines.Length, lines[0].Length];
	var grid = new int[lines.Length, lines[0].Length];

	for (int i = 0; i < lines.Length; i++)
	{
		for (int j = 0; j < lines[i].Length; j++)
		{
			var row = lines[i];
			grid[i, j] = int.Parse(row[j].ToString()) == 9 ? -1 : 0;
		}
	}
prevgrid = (int[,])grid.Clone();
	//grid.Dump();

	//Go through the entire array, set self to min of neighboors, if 0 use ++basincount.  any new region will have a unique lowest number
	//Do until no changes have happened, multiple passes will eventually get all touching reagions to be the name as the initial number
	var basincount = 0;
	var changeHappend = true;
	while (changeHappend)
	{
		changeHappend = false;
		for (int x = 0; x < grid.GetLength(1); x++)
		{
			for (int y = 0; y < grid.GetLength(0); y++)
			{
				if (grid[y, x] != -1)
				{
					var neighbors = new List<int>();

					//check up
					if (y - 1 >= 0)
					{
						neighbors.Add(grid[y - 1, x]);
					}

					//check down
					if (y + 1 < grid.GetLength(0))
					{
						neighbors.Add(grid[y + 1, x]);
					}

					//check left
					if (x - 1 >= 0)
					{
						neighbors.Add(grid[y, x - 1]);
					}

					//check right
					if (x + 1 < grid.GetLength(1))
					{
						neighbors.Add(grid[y, x + 1]);
					}
					var nMin = neighbors.Where(n => n > 0).DefaultIfEmpty(0).Min();
					var newchange = nMin == 0 ? ++basincount : nMin;


					if (grid[y, x] != newchange)
					{
						grid[y, x] = newchange;
						changeHappend = true;
					}

				}

				
			}
		}
		//Util.Dif(prevgrid,grid).Dump();
		prevgrid = (int[,])grid.Clone();
		
	}

	//grid.Dump();

	var basins = new Dictionary<int, int>();

	for (int x = 0; x < grid.GetLength(1); x++)
	{
		for (int y = 0; y < grid.GetLength(0); y++)
		{
			if (grid[y, x] != -1)
			{
				if (basins.ContainsKey(grid[y, x]))
				{
					basins[grid[y, x]]++;
				}
				else
				{
					basins.Add(grid[y, x], 1);
				}
			}

		}
	}
	
	var basinscolor = new object[lines.Length, lines[0].Length];

	for (int x = 0; x < grid.GetLength(1); x++)
	{
		for (int y = 0; y < grid.GetLength(0); y++)
		{
			if (grid[y, x] != -1)
			{
				basinscolor[y,x] = Util.WithStyle(grid[y, x],"background-color:blue");
			}else
			{
				basinscolor[y,x] = Util.WithStyle(".","background-color:black");
			}
		}
	}
	basinscolor.Dump();
	//basins.OrderByDescending(b => b.Value).Dump();
	var total = basins.OrderByDescending(b => b.Value).Select(b => b.Value).Take(3).Aggregate(1, (a, b) => a * b);
	return total;
	
	
}

private bool gridHasZerosLeft(int[,] grid)
{
	for (int i = 0; i < grid.GetLength(1); i++)
	{
		for (int j = 0; j < grid.GetLength(0); j++)
		{
			if (grid[i, j] == 0)
			{
				return true;
			}
		}
	}

	return false;
}
private List<string> getNeighborsNoRidges(string point, int[,] grid)
{
	var x = int.Parse(point.Split(',')[0]);
	var y = int.Parse(point.Split(',')[1]);

	var currentdepth = grid[y, x];

	var neighbors = new List<string>();
	//check up
	if (y - 1 >= 0 && grid[y - 1, x] != 9 && grid[y - 1, x] >= currentdepth)
	{
		neighbors.Add($"{x},{y - 1}");
	}

	//check down
	if (y + 1 < grid.GetLength(0) && grid[y + 1, x] != 9 && grid[y + 1, x] >= currentdepth)
	{
		neighbors.Add($"{x},{y + 1}");
	}

	//check left
	if (x - 1 >= 0 && grid[y, x - 1] != 9 && grid[y, x - 1] >= currentdepth)
	{
		neighbors.Add($"{x - 1},{y}");
	}

	//check right
	if (x + 1 < grid.GetLength(1) && grid[y, x + 1] != 9 && grid[y, x + 1] >= currentdepth)
	{
		neighbors.Add($"{x + 1},{y}");
	}

	var newNeighbors = new List<string>();

	if (neighbors.Count() == 0)
	{
		return neighbors;
	}
	else
	{
		foreach (var n in neighbors)
		{
			newNeighbors.AddRange(getNeighborsNoRidges(n, grid));
		}
		neighbors.AddRange(newNeighbors);
		neighbors.Add(point);
		return neighbors;
	}

}