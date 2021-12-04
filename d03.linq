<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

#load "utilities.linq"

async Task Main()
{
	//testing
	Debug.WriteLine("Beginning Tests");
	Debug.Assert(await p1(true) == 198);
	Debug.Assert(await p2(true) == 230);
	Debug.WriteLine("Tests Complete");

	await p1().Dump();
	await p2().Dump();
}

// You can define other methods, fields, classes and namespaces here
public async Task<int> p1(bool test = false)
{
	var lines = await GetLinesFromFile("d03", test);
	
	var grid = new int[lines.Length];
	
	var gamma = "";
	var epsilon = "";
	
	for (int i = 0; i < lines.First().Length; i++)
	{
		var s = new string(lines.Select(l => l[i]).ToArray());
		
		gamma += (s.Count(l => l == '1') > s.Count(l => l == '0')) ? "1" : "0";
		epsilon += (s.Count(l => l == '1') > s.Count(l => l == '0')) ? "0" : "1";
	}
	
	return Convert.ToInt32(gamma, 2) * Convert.ToInt32(epsilon, 2);
}

public async Task<int> p2(bool test = false)
{
	var lines = (await GetLinesFromFile("d03", test)).ToList();
	
	var oxy = "";
	var co2 = "";
	
	var ind = 0;
	while (lines.Count > 1)
	{
		var s = new string(lines.Select(l => l[ind]).ToArray());
		
		lines.RemoveAll(x=>x[ind] != (s.Count(l => l == '1') >= s.Count(l => l == '0') ? '1' : '0'));
		
		ind++;
	}
	
	oxy = lines.First();

	lines = (await GetLinesFromFile("d03", test)).ToList();
	ind = 0;
	
	while (lines.Count > 1)
	{
		var s = new string(lines.Select(l => l[ind]).ToArray());

		lines.RemoveAll(x => x[ind] != (s.Count(l => l == '1') >= s.Count(l => l == '0') ? '0' : '1'));

		ind++;
	}
	
	co2 = lines.First();

	return Convert.ToInt32(oxy, 2) * Convert.ToInt32(co2, 2);
}