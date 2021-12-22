<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

#load "utilities.linq"

async Task Main()
{
	//testing
	Debug.WriteLine("Beginning Tests");
	Debug.Assert(await p1(true) == 0);
	Debug.Assert(await p2(true) == 0);
	Debug.WriteLine("Tests Complete");

	await p1().Dump();
	await p2().Dump();
}

// You can define other methods, fields, classes and namespaces here
public async Task<int> p1(bool test = false)
{
	var lines = await GetLinesFromFile("d19", test);

	var Scanners = new List<Scanner>();

	for (int i = 0; i < lines.Length; i++)
	{
		if (lines[i].StartsWith("---"))
		{
			var scan = new Scanner() { x = 0, y = 0, Beacons = new List<UserQuery.point>(), name = lines[i] };
			i++;
			while (lines[i].Length > 0)
			{
				scan.Beacons.Add(new point(lines[i].Split(',')[0], lines[i].Split(',')[1], lines[i].Split(',')[2]));
				i++;
			}
			Scanners.Add(scan);
		}
	}

	Scanners.Dump();

	var KnownPoints = new List<point>();

	Scanners[0].Beacons.ForEach(b => KnownPoints.Add(new point(b.X, b.Y, b.Z)));

	// all orientations, apply 3 Z orientations
	// All horizontal Orientations
	// Initial orientation
	// Y
	// Y2
	// Y3
	// Top of cube
	// X
	// Bottom of cube
	// -X
	foreach (var kp in KnownPoints)
	{
		foreach (var beacon in Scanners[1].Beacons)
		{
			var dif = point.Diff(kp, beacon);
			
			//using diff between kp and beacon, apply to all other beacons and see if at least 12 match
		}
	}



	return 0;
}

public async Task<int> p2(bool test = false)
{
	var lines = await GetLinesFromFile("d19", test);

	return 0;
}

private class point
{
	public int X { get; set; }
	public int Y { get; set; }
	public int Z { get; set; }

	public point(int x, int y, int z)
	{
		X = x;
		Y = y;
		Z = z;
	}

	public point(string x, string y, string z)
	{
		X = int.Parse(x);
		Y = int.Parse(y);
		Z = int.Parse(z);
	}

	public static point Diff(point p1, point p2)
	{
		return new point(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
	}
}

private class Scanner
{
	public string name { get; set; }
	public int x { get; set; }
	public int y { get; set; }

	public List<point> Beacons = new();
}

private List<point> RotateScanner(List<point> Beacons, point rotation)
{
	var rotFunc = tfp[rotation];
	var newOrientations = Beacons.Select(b => rotFunc(b)).ToList();

	return newOrientations;
}

private static Dictionary<point, Func<point, point>> tfp = new Dictionary<point, Func<point, point>>()
{
	{
		new point(-1,0,0),
		(p) => new point(p.X, -p.Z, p.Y)
	},
	{
		new point(1,0,0),
		(p) => new point(p.X, p.Z, -p.Y)
	},
	{
		new point(0,-1,0),
		(p)=> new point(-p.Z,p.Y,p.X)
	},
	{
		new point(0,1,0),
		(p)=> new point(p.Z,p.Y,-p.X)
	},
	{
		new point(0,0,-1),
		(p)=> new point(-p.Y,p.X,p.Z)
	},
	{
		new point(0,0,1),
		(p)=> new point(p.Y,-p.X,p.Z)
	}
};