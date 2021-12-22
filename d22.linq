<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

#load "utilities.linq"

async Task Main()
{
	//testing
	Debug.WriteLine("Beginning Tests");
	//Debug.Assert(await p1(true) == 590784);
	Debug.Assert(await p2(true) == 2758514936282235);
	Debug.WriteLine("Tests Complete");

	//await p1().Dump();
	await p2().Dump();
}

// You can define other methods, fields, classes and namespaces here
public async Task<int> p1(bool test = false)
{
	var lines = await GetLinesFromFile("d22", test);

	var area = new Dictionary<string, int>();

	foreach (var line in lines)
	{
		var lineSplit = line.Split(" ");
		var mode = lineSplit[0] == "on" ? 1 : 0;

		var regions = GetRegion(lineSplit[1]);

		foreach (var r in regions)
		{
			area[r] = mode;
		}
	}

	var count = area.Count(a => a.Value == 1);
	return count;
}

public async Task<long> p2(bool test = false)
{
	var lines = await GetLinesFromFile("d22", test);

	var instructions = new List<(int xmin, int xmax, int ymin, int ymax, int zmin, int zmax, bool turnon)>();

	var cubes = new Dictionary<(int xmin, int xmax, int ymin, int ymax, int zmin, int zmax), long>();
	
	foreach (var line in lines)
	{
		var onoff = line.Split(" ")[0];
		var inst = line.Split(" ")[1];
		var lineSplit = inst.Split(",");

		var xstart = int.Parse(lineSplit[0].Remove(0, 2).Split("..")[0]);
		var xend = int.Parse(lineSplit[0].Remove(0, 2).Split("..")[1]);

		var ystart = int.Parse(lineSplit[1].Remove(0, 2).Split("..")[0]);
		var yend = int.Parse(lineSplit[1].Remove(0, 2).Split("..")[1]);

		var zstart = int.Parse(lineSplit[2].Remove(0, 2).Split("..")[0]);
		var zend = int.Parse(lineSplit[2].Remove(0, 2).Split("..")[1]);

		instructions.Add((xstart, xend, ystart, yend, zstart, zend, onoff == "on"));
	}

	//create cuboids
	foreach (var ins in instructions)
	{
		(int minX, int maxX, int minY, int maxY, int minZ, int maxZ, bool turnOn) = ins;
		long newSign = turnOn ? 1 : -1;

		Dictionary<(int minX, int maxX, int minY, int maxY, int minZ, int maxZ), long> newCuboids = new();

		var newCuboid = (minX, maxX, minY, maxY, minZ, maxZ);

		foreach (var kvp in cubes)
		{
			(int minX2, int maxX2, int minY2, int maxY2, int minZ2, int maxZ2) = kvp.Key;
			var curSign = kvp.Value;

			//These determine the overlapping region, it even grabs complete interior bits.
			int tmpMinX = Math.Max(minX, minX2);
			int tmpMaxX = Math.Min(maxX, maxX2);
			int tmpMinY = Math.Max(minY, minY2);
			int tmpMaxY = Math.Min(maxY, maxY2);
			int tmpMinZ = Math.Max(minZ, minZ2);
			int tmpMaxZ = Math.Min(maxZ, maxZ2);

			var tmpCuboid = (tmpMinX, tmpMaxX, tmpMinY, tmpMaxY, tmpMinZ, tmpMaxZ);

			//Basically, do we have a normal cuboid?
			//If so it is an intersection, and we must cubtract it from our final count
			//If the new cuboid from the instructions is on we need to remove it to avoid double counting
			//If it's an off, we need to remove it to make sure to turn things off. 
			if (tmpMinX <= tmpMaxX && tmpMinY <= tmpMaxY && tmpMinZ <= tmpMaxZ) newCuboids[tmpCuboid] = newCuboids.GetValueOrDefault(tmpCuboid, 0) - curSign;
		}
		//If we are in fact turning this on, we want to make sure to do that, we don't just assign to one in case a previous collision already set things.
		if (newSign == 1) newCuboids[newCuboid] = newCuboids.GetValueOrDefault(newCuboid, 0) + newSign;

		//Add or update the value of the cuboids.
		foreach (var kvp in newCuboids)
		{
			cubes[kvp.Key] = cubes.GetValueOrDefault(kvp.Key, 0) + kvp.Value;
		}
	}

	var value = cubes.Sum(a => (a.Key.xmax - a.Key.xmin + 1L) * (a.Key.ymax - a.Key.ymin + 1) * (a.Key.zmax - a.Key.zmin + 1) * a.Value);

	return value;
}

private List<string> GetRegion(string inst)
{
	var region = new List<string>();

	var lineSplit = inst.Split(",");

	var xstart = int.Parse(lineSplit[0].Remove(0, 2).Split("..")[0]);
	var xend = int.Parse(lineSplit[0].Remove(0, 2).Split("..")[1]);

	var ystart = int.Parse(lineSplit[1].Remove(0, 2).Split("..")[0]);
	var yend = int.Parse(lineSplit[1].Remove(0, 2).Split("..")[1]);

	var zstart = int.Parse(lineSplit[2].Remove(0, 2).Split("..")[0]);
	var zend = int.Parse(lineSplit[2].Remove(0, 2).Split("..")[1]);

	for (int x = xstart; x <= xend; x++)
	{
		for (int y = ystart; y <= yend; y++)
		{
			for (int z = zstart; z <= zend; z++)
			{
				region.Add($"{x},{y},{z}");
			}
		}
	}

	return region;
}

