<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

#load "utilities.linq"

async Task Main()
{
	//testing
	Debug.WriteLine("Beginning Tests");
	Debug.Assert(await p1(true) == 17);
	Debug.Assert(await p2(true) == 0);
	Debug.WriteLine("Tests Complete");

	await p1().Dump();
	await p2().Dump();
}

// You can define other methods, fields, classes and namespaces here
public async Task<int> p1(bool test = false)
{
	var lines = await GetLinesFromFile("d13", test);

	var grid = new Dictionary<string, int>();
	var instructions = new List<string>();

	var mode = 1;
	foreach (var line in lines)
	{
		if (line.Length == 0)
		{
			mode = 2;
			continue;
		}

		switch (mode)
		{
			case 1:
				grid.Add(line, 1);
				break;
			case 2:
				instructions.Add(line.Split(" ")[2]);
				break;
		}
	}
	
	Fold(grid,instructions.First());
	
	return grid.Count();
}

public async Task<int> p2(bool test = false)
{
	var lines = await GetLinesFromFile("d13", test);

	var grid = new Dictionary<string, int>();
	var instructions = new List<string>();

	var mode = 1;
	foreach (var line in lines)
	{
		if (line.Length == 0)
		{
			mode = 2;
			continue;
		}

		switch (mode)
		{
			case 1:
				grid.Add(line, 1);
				break;
			case 2:
				instructions.Add(line.Split(" ")[2]);
				break;
		}
	}
	
	foreach (var inst in instructions)
	{
		Fold(grid,inst);
	}
	
	var array = new char[6,40];
	
	foreach (var cell in grid)
	{
		array[int.Parse(cell.Key.Split(",")[1]),int.Parse(cell.Key.Split(",")[0])] = '#';
	}
	
	array.Dump();
	
	return 0;
}

private Dictionary<string, int> Fold(Dictionary<string, int> grid, string instruction)
{
	if (instruction.StartsWith("y"))
	{
		//fold vertically
		//get max
		//it looks like all folds are in half so take instruction and get double;
		var instInt = int.Parse(instruction.Split('=')[1]);
		int max = (instInt * 2);
		int min = 0;
		var removeList = new List<string>();
		var addlist = new List<string>();
		//working way back, find all items in max row and put them into the opposite row
		for (int i = max; i > instInt; i--)
		{
			foreach (var cell in grid.Where(x => x.Key.Contains($",{i}")))
			{
				var oppositeCell = $"{cell.Key.Split(",")[0]},{min}";
				addlist.Add(oppositeCell);
				removeList.Add(cell.Key);
			}
			min++;
		}

		removeList.ForEach(l => grid.Remove(l));
		foreach (var add in addlist)
		{
			if (grid.ContainsKey(add))
			{
				grid[add]++;
			}
			else
			{
				grid.Add(add, 1);
			}
		}
	}else
	{
		//fold vertically
		//get max
		//it looks like all folds are in half so take instruction and get double;
		var instInt = int.Parse(instruction.Split('=')[1]);
		int max = (instInt * 2);
		int min = 0;
		var removeList = new List<string>();
		var addlist = new List<string>();
		//working way back, find all items in max row and put them into the opposite row
		for (int i = max; i > instInt; i--)
		{
			foreach (var cell in grid.Where(x => x.Key.Contains($"{i},")))
			{
				var oppositeCell = $"{min},{cell.Key.Split(",")[1]}";
				addlist.Add(oppositeCell);
				removeList.Add(cell.Key);
			}
			min++;
		}

		removeList.ForEach(l => grid.Remove(l));
		foreach (var add in addlist)
		{
			if (grid.ContainsKey(add))
			{
				grid[add]++;
			}
			else
			{
				grid.Add(add, 1);
			}
		}
	}


	return grid;
}