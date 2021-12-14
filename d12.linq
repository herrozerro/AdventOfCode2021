<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

#load "utilities.linq"

async Task Main()
{
	//testing
	Debug.WriteLine("Beginning Tests");
	Debug.Assert(await p1(true) == 10);
	Debug.Assert(await p2(true) == 36);
	Debug.WriteLine("Tests Complete");

	await p1().Dump();
	await p2().Dump();
}

// You can define other methods, fields, classes and namespaces here
public async Task<int> p1(bool test = false)
{
	var lines = await GetLinesFromFile("d12", test);

	var nodes = new List<KeyValuePair<string, string>>();

	foreach (var line in lines)
	{
		nodes.Add(new KeyValuePair<string,string>(line.Split('-')[0], line.Split('-')[1]));
		nodes.Add(new KeyValuePair<string,string>(line.Split('-')[1], line.Split('-')[0]));
	}

	var paths = pathFind(nodes,new string[1]{"start"});
	
	//paths.Dump();
	
	return paths.Count(p => p.Contains("end"));
}

public async Task<int> p2(bool test = false)
{
	var lines = await GetLinesFromFile("d12", test);

	var nodes = new List<KeyValuePair<string, string>>();

	foreach (var line in lines)
	{
		nodes.Add(new KeyValuePair<string, string>(line.Split('-')[0], line.Split('-')[1]));
		nodes.Add(new KeyValuePair<string, string>(line.Split('-')[1], line.Split('-')[0]));
	}
	
	nodes.RemoveAll(x=>x.Value == "start");
	
	//nodes.OrderBy(n => n.Key).Dump();
	
	var paths = pathFindSmallTwice(nodes.OrderBy(n => n.Key).ToList(), new string[1] { "start" });

	return paths.Count();
}

private List<List<string>> pathFindSmallTwice(List<KeyValuePair<string, string>> nodes, string[] origin, bool visitedSmallTwice = false)
{
	
	var paths = new List<List<string>>();
	
	if (origin.Last() == "end")
	{
		paths.Add(origin.ToList());
		return paths;
	}

	if (!visitedSmallTwice)
	{
		visitedSmallTwice = origin.GroupBy(o => o).Any(o => o.Count() == 2 && o.Key.All(p => char.IsLower(p)));
	}

	foreach (var start in nodes.Where(n => n.Key == origin.Last()))
	{
		//check if small cave and if visited before
		if (start.Value.All(v => char.IsLower(v)) && origin.Count(o=> o == start.Value) == 2 || start.Value.All(v => char.IsLower(v)) && origin.Count(o=> o == start.Value) == 1 && visitedSmallTwice)
		{
			continue;
		}
		//else continue pathchecking
		var prevpath = origin.Concat(new string[]{start.Value}).ToArray();
		paths.AddRange(pathFindSmallTwice(nodes, prevpath, visitedSmallTwice));
	}
	
	return paths;
}

private List<List<string>> pathFind(List<KeyValuePair<string, string>> nodes, string[] origin)
{
	var paths = new List<List<string>>();

	if (origin.Last() == "end")
	{
		paths.Add(origin.ToList());
		return paths;
	}

	foreach (var start in nodes.Where(n => n.Key == origin.Last()))
	{
		//check if small cave and if visited before
		if (start.Value.All(v => char.IsLower(v)) && origin.Contains(start.Value))
		{
			continue;
		}
		//else continue pathchecking
		var prevpath = origin.Concat(new string[] { start.Value }).ToArray();
		paths.AddRange(pathFind(nodes, prevpath));
	}

	return paths;
}