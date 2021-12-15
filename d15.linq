<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

#load "utilities.linq"

async Task Main()
{
	//testing
	Debug.WriteLine("Beginning Tests");
	Debug.Assert(await p1(true) == 40);
	Debug.Assert(await p2(true) == 315);
	Debug.WriteLine("Tests Complete");

	await p1().Dump();
	await p2().Dump();
	"should be 2840".Dump();
}

// You can define other methods, fields, classes and namespaces here
public async Task<int> p1(bool test = false)
{
	var lines = await GetLinesFromFile("d15", test);

	var grid = new Dictionary<string, (int hazard, int cost)>();

	for (int y = 0; y < lines.Length; y++)
	{
		for (int x = 0; x < lines[0].Length; x++)
		{
			grid[$"{x},{y}"] = (int.Parse(lines[y][x].ToString()), 0);
		}
	}



	var min = "0,0";
	var max = $"{lines.Length - 1},{lines[0].Length - 1}";

	for (int y = lines.Length - 1; y >= 0; y--)
	{
		for (int x = lines[0].Length - 1; x >= 0; x--)
		{
			var cell = $"{x},{y}";
			var right = $"{x + 1},{y}";
			var down = $"{x},{y + 1}";

			var rightCost = 0;
			var downCost = 0;

			rightCost = !grid.ContainsKey(right) ? 0 : grid[right].cost + grid[right].hazard;
			downCost = !grid.ContainsKey(down) ? 0 : grid[down].cost + grid[down].hazard;


			if (rightCost > 0 && rightCost < downCost || rightCost > 0 && downCost == 0)
			{
				grid[cell] = (grid[cell].hazard, rightCost);
			}
			if (downCost > 0 && downCost < rightCost || downCost > 0 && rightCost == 0)
			{
				grid[cell] = (grid[cell].hazard, downCost);
			}
			if (rightCost == downCost)
			{
				grid[cell] = (grid[cell].hazard, downCost);
			}
		}
		//DisplayGrid(grid);
	}

	//if (!test)
	//{
	//	DisplayGrid(grid);
	//}

	return (grid["0,0"].cost - grid["0,0"].hazard) + grid[max].hazard;
}

public async Task<int> p2(bool test = false)
{
	var lines = await GetLinesFromFile("d15", test);

	var grid = new Dictionary<string, (int hazard, int cost)>();



	for (int y = 0; y < lines.Length; y++)
	{
		for (int x = 0; x < lines[0].Length; x++)
		{
			grid[$"{x},{y}"] = (int.Parse(lines[y][x].ToString()), 0);
		}
	}

	grid = ExpandMap(grid, 5, lines.Length);

	var min = "0,0";
	var max = $"{lines.Length - 1},{lines[0].Length - 1}";

	var maxy = lines.Length * 5;
	var maxx = lines[0].Length * 5;

	for (int y = maxy - 1; y >= 0; y--)
	{
		for (int x = maxx - 1; x >= 0; x--)
		{
			var cell = $"{x},{y}";
			var right = $"{x + 1},{y}";
			var down = $"{x},{y + 1}";

			var rightCost = 0;
			var downCost = 0;

			rightCost = !grid.ContainsKey(right) ? 0 : grid[right].cost + grid[right].hazard;
			downCost = !grid.ContainsKey(down) ? 0 : grid[down].cost + grid[down].hazard;


			if (rightCost > 0 && rightCost < downCost || rightCost > 0 && downCost == 0)
			{
				grid[cell] = (grid[cell].hazard, rightCost);
			}
			if (downCost > 0 && downCost < rightCost || downCost > 0 && rightCost == 0)
			{
				grid[cell] = (grid[cell].hazard, downCost);
			}
			if (rightCost == downCost)
			{
				grid[cell] = (grid[cell].hazard, downCost);
			}
		}
		//DisplayGrid(grid);
	}

	if (!test)
	{
		DisplayGrid(grid);
	}

	return (grid["0,0"].cost - grid["0,0"].hazard) + grid[max].hazard;
}

private void DisplayGrid(Dictionary<string, (int hazard, int cost)> grid)
{
	var s = new string[100, 100];
	for (int y = 0; y < 100; y++)
	{
		for (int x = 0; x < 100; x++)
		{
			var i = grid[$"{x},{y}"];
			s[y, x] = $"{i.hazard} - {i.cost}";
		}
	}

	s.Dump();
}

private Dictionary<string, (int hazard, int cost)> ExpandMap(Dictionary<string, (int hazard, int cost)> grid, int size, int gridSize)
{
	for (int ym = 0; ym < size; ym++)
	{
		for (int xm = 0; xm < size; xm++)
		{
			if (xm == 0 && ym == 0)
			{
				continue;
			}
			var xoffset = xm * gridSize;
			var yoffset = ym * gridSize;

			var OffsetAdd = ym + xm;

			for (int y = 0; y < gridSize; y++)
			{
				for (int x = 0; x < gridSize; x++)
				{
					var newCell = $"{x + xoffset},{y + yoffset}";
					var cell = $"{x},{y}";
					var value = (grid[cell].hazard + OffsetAdd) % 9;
					grid[newCell] = (value == 0 ? 9 : value, 0);
				}
			}
		}
	}

	return grid;
}

