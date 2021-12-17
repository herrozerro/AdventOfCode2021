<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

#load "utilities.linq"

async Task Main()
{
	//testing
	Debug.WriteLine("Beginning Tests");
	//Debug.Assert(await p1(true) == 45);
	//Debug.Assert(await p2(true) == 0);
	Debug.WriteLine("Tests Complete");

	await p1().Dump();
	await p2().Dump();
}

// You can define other methods, fields, classes and namespaces here
public async Task<int> p1(bool test = false)
{
	var lines = await GetStringFromFile("d17", test);
	//6,9
//x=236..262, y=-78..-58
	var maxy = 0;
	var ProbPos = (0, 0);

	for (int x = 0; x < 1000; x++)
	{
		for (int y = 0; y < 1000; y++)
		{
			maxy = Math.Max(maxy, SimiulateProbe(x,y,(236,262,-78,-58)));
		}
	}


	return maxy;
}

public async Task<int> p2(bool test = false)
{
	var lines = await GetStringFromFile("d17", test);
	//6,9
	//x=236..262, y=-78..-58
	//x=20..30, y=-10..-5
	var successes = new List<string>();
	
	//SimiulateProbe(29, -10, (20, 30, -10, -5)).Dump();
	
	for (int x = 0; x < 1000; x++)
	{
		for (int y = -1000; y < 1000; y++)
		{
			if (SimiulateProbe2(x, y, (236, 262, -78, -58)) > 0)
			{
				successes.Add($"{x},{y}");
			}
		}
	}

	return successes.Count();
}

private int SimiulateProbe(int xvel, int yvel, (int xmin, int xmax, int ymin, int ymax) range)
{
	(int x, int y) ProbPos = new(0, 0);

	int ymax = 0;

	while (ProbPos.x < range.xmax && ProbPos.y > range.ymin)
	{
		ProbPos.x += xvel;
		ProbPos.y += yvel;

		yvel -= 1;
		if (xvel > 0)
		{
			xvel -= 1;
		}
		
		
		ymax = Math.Max(ymax, ProbPos.y);

		if (isInTargetArea(ProbPos, range))
		{
			return ymax;
		}
	}
	
	//missed the target area
	return 0;
}

private int SimiulateProbe2(int xvel, int yvel, (int xmin, int xmax, int ymin, int ymax) range)
{
	(int x, int y) ProbPos = new(0, 0);

	int ymax = 0;

	while (ProbPos.x < range.xmax && ProbPos.y > range.ymin)
	{
		ProbPos.x += xvel;
		ProbPos.y += yvel;

		yvel -= 1;
		if (xvel > 0)
		{
			xvel -= 1;
		}


		ymax = Math.Max(ymax, ProbPos.y);

		if (isInTargetArea(ProbPos, range))
		{
			return 1;
		}
	}

	//missed the target area
	return 0;
}

private bool isInTargetArea((int x, int y) probPos, (int xmin, int xmax, int ymin, int ymax) range)
{
	var isInX = probPos.x <= range.xmax && probPos.x >= range.xmin;
	var isInY = probPos.y >= range.ymin && probPos.y <= range.ymax;
	return (isInX && isInY);
}