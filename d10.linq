<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

#load "utilities.linq"

async Task Main()
{
	//testing
	Debug.WriteLine("Beginning Tests");
	Debug.Assert(await p1(true) == 26397);
	Debug.Assert(await p2(true) == 288957);
	Debug.WriteLine("Tests Complete");

	await p1().Dump();
	await p2().Dump();
}

// You can define other methods, fields, classes and namespaces here
public async Task<int> p1(bool test = false)
{
	var lines = await GetLinesFromFile("d10", test);
	
	var stack = new Stack<char>();

	Dictionary<char, char> matches = new Dictionary<char, char>();
	matches.Add('(', ')');
	matches.Add('[', ']');
	matches.Add('{', '}');
	matches.Add('<', '>');

	var score = new Dictionary<char, int>();
	score.Add(')', 3);
	score.Add(']', 57);
	score.Add('}', 1197);
	score.Add('>', 25137);

	var unexpected = new List<char>();
	foreach (var line in lines)
	{
		foreach (var c in line)
		{
			if (matches.ContainsKey(c))
			{
				stack.Push(c);
			}
			else
			{
				if (matches[stack.Peek()] == c)
				{
					stack.Pop();
				}
				else
				{
					unexpected.Add(c);
					break;
				}
			}
		}
	}

	return unexpected.Sum(u => score[u]);
}

public async Task<UInt64> p2(bool test = false)
{
	var lines = await GetLinesFromFile("d10", test);

	

	Dictionary<char, char> matches = new Dictionary<char, char>();
	matches.Add('(', ')');
	matches.Add('[', ']');
	matches.Add('{', '}');
	matches.Add('<', '>');

	var score = new Dictionary<char, int>();
	score.Add(')', 1);
	score.Add(']', 2);
	score.Add('}', 3);
	score.Add('>', 4);
	
	var scores = new List<UInt64>();
	
	var incomplete = new List<string>();
	
	foreach (var line in lines)
	{
		var stack = new Stack<char>();
		
		var isvalid = true;
		
		foreach (var c in line)
		{
			if (matches.ContainsKey(c))
			{
				stack.Push(c);
			}
			else
			{
				if (matches[stack.Peek()] == c)
				{
					stack.Pop();
				}
				else
				{
					isvalid = false;
					break;
				}
			}
		}
		
		if (!isvalid)
		{
			continue;
		}
		
		//complete incomplete lines
		var ending = "";
		
		while (stack.Count() > 0)
		{
			ending += matches[stack.Pop()];
		}

		
		UInt64 s = 0;
		foreach (var e in ending)
		{			
			s *= 5;
			s += (UInt64)score[e];
		}
		scores.Add(s);
	}
	
	var finalscore = scores.OrderByDescending(x=>x).Dump().Skip((scores.Count())/2).First();
	
	return finalscore;
}