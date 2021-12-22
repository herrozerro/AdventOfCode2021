<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

#load "utilities.linq"

async Task Main()
{
	//testing
	Debug.WriteLine("Beginning Tests");
	//Debug.Assert(await p1(true) == 35);
	//Debug.Assert(await p2(true) == 3351);
	Debug.WriteLine("Tests Complete");

	//await p1().Dump();
	await p2().Dump();
}

// You can define other methods, fields, classes and namespaces here
public async Task<int> p1(bool test = false)
{
	var lines = await GetLinesFromFile("d20", test);

	var key = lines[0];

	var map = new char[1000, 1000];

	for (int i = 0; i < 1000; i++)
		for (int j = 0; j < 1000; j++)
			map[i, j] = '.';

	var startx = 450;
	var starty = 450;

	foreach (var line in lines.Skip(2))
	{
		foreach (var chr in line)
		{
			map[starty, startx] = chr;
			startx++;
		}
		startx = 450;
		starty++;
	}
	iterate(map, key, true, 1000);
	iterate(map, key, false,1000);

	map.Dump();
	var lit = 0;

	for (int i = 0; i < 1000; i++)
		for (int j = 0; j < 1000; j++)
			if (map[i, j] == '#')
				lit++;

	return lit;
}

public async Task<int> p2(bool test = false)
{
	var gridsize = 250;
	var lines = await GetLinesFromFile("d20", test);

	var key = lines[0];

	var map = new char[gridsize, gridsize];

	for (int i = 0; i < gridsize; i++)
		for (int j = 0; j < gridsize; j++)
			map[i, j] = '.';

	var startx = (gridsize / 2)-50;
	var starty = (gridsize / 2)-50;

	foreach (var line in lines.Skip(2))
	{
		foreach (var chr in line)
		{
			map[starty, startx] = chr;
			startx++;
		}
		startx = gridsize/2;
		starty++;
	}

	for (int i = 1; i < 51; i++)
	{
		iterate(map, key, i % 2 == 0, gridsize);
	}

	map.Dump();

	//for (int i = 0; i < gridsize; i++)
	//{
	//	for (int j = 0; j < gridsize; j++)
	//	{
	//		Console.Write(map[j,i]);
	//	}
	//	Console.WriteLine();
	//}
	var lit = 0;

	for (int i = 0; i < gridsize; i++)
		for (int j = 0; j < gridsize; j++)
			if (map[i, j] == '#')
				lit++;

	return lit;
}

private void iterate(char[,] map, string key, bool odd,int gridsize)
{
	var changes = new List<(int x, int y, char change)>();
	for (int x = 1; x < gridsize-1; x++)
	{
		for (int y = 1; y < gridsize-1; y++)
		{
			var indexString = "";
			indexString += map[y - 1, x - 1] == '.' ? "0" : "1";
			indexString += map[y - 1, x] == '.' ? "0" : "1";
			indexString += map[y - 1, x + 1] == '.' ? "0" : "1";
			indexString += map[y, x - 1] == '.' ? "0" : "1";
			indexString += map[y, x] == '.' ? "0" : "1";
			indexString += map[y, x + 1] == '.' ? "0" : "1";
			indexString += map[y + 1, x - 1] == '.' ? "0" : "1";
			indexString += map[y + 1, x] == '.' ? "0" : "1";
			indexString += map[y + 1, x + 1] == '.' ? "0" : "1";

			var index = Convert.ToInt16(indexString, 2);

			if (map[y, x] != key[index])
			{
				changes.Add((x, y, key[index]));
			}
		}
	}

	var universe = odd ? '.' : '#';
	//change outer ring as well
	for (int i = 0; i < gridsize; i++)
	{
		changes.Add((i, 0, universe));
		changes.Add((0, i, universe));
		changes.Add((i, gridsize-1, universe));
		changes.Add((gridsize-1, i, universe));
	}

	foreach (var ch in changes)
	{
		map[ch.y, ch.x] = ch.change;
	}
	//Util.ClearResults();
	//for (int i = 0; i < gridsize; i++)
	//{
	//	for (int j = 0; j < gridsize; j++)
	//	{
	//		Console.Write(map[j,i]);
	//	}
	//	Console.WriteLine();
	//}
}