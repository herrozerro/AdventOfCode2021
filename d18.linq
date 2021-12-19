<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

#load "utilities.linq"

async Task Main()
{

	//testing
	Debug.WriteLine("Beginning Tests");
	Debug.Assert(await p1(true) == 4140);
	Debug.Assert(await p2(true) == 3993);
	Debug.WriteLine("Tests Complete");

	await p1().Dump();
	await p2().Dump();
}

// You can define other methods, fields, classes and namespaces here
public async Task<ulong> p1(bool test = false)
{
	var lines = await GetLinesFromFile("d18", test);

	var q = new Queue<string>(lines);

	var number = "";
	//mode 0 means explode, mode 1 = split  whenever an operation happens revert to explode
	var mode = 0;

	number = q.Dequeue();

	var iterator = 1;
	while (true)
	{
		while (CanExplode(number) || CanSplit(number))
		{
			switch (mode)
			{
				case 0: //explode
					if (!CanExplode(number))
					{
						mode++;
						continue;
					}
					number = Explode(number);
					mode = 0;
					break;
				case 1: //Split
					number = Split(number);
					mode = 0;
					break;
			}
		}

		if (q.Count == 0)
		{
			break;
		}

		number = $"[{number},{q.Dequeue()}]";
	}

	return Magnitude(number).Dump();
}

public async Task<ulong> p2(bool test = false)
{
	var lines = await GetLinesFromFile("d18", test);

	var q = new Queue<string>(lines);

	var number = "";
	//mode 0 means explode, mode 1 = split  whenever an operation happens revert to explode
	var mode = 0;
	
	var maxmag = 0UL;
	
	for (int i = 0; i < lines.Length; i++)
	{
		for (int j = lines.Length -1; j >= 0; j--)
		{
			if (i == j)
			{
				continue;
			}
			number = $"[{lines[i]},{lines[j]}]";
			while (CanExplode(number) || CanSplit(number))
			{
				switch (mode)
				{
					case 0: //explode
						if (!CanExplode(number))
						{
							mode++;
							continue;
						}
						number = Explode(number);
						mode = 0;
						break;
					case 1: //Split
						number = Split(number);
						mode = 0;
						break;
				}
			}
			var mag = Magnitude(number);
			maxmag = maxmag > mag ? maxmag : mag;
		}

	}

	for (int i = lines.Length -1; i >= 0; i--)
	{
		for (int j = 0; j < lines.Length; j++ )
		{
			if (i == j)
			{
				continue;
			}
			number = $"[{lines[i]},{lines[j]}]";
			while (CanExplode(number) || CanSplit(number))
			{
				switch (mode)
				{
					case 0: //explode
						if (!CanExplode(number))
						{
							mode++;
							continue;
						}
						number = Explode(number);
						mode = 0;
						break;
					case 1: //Split
						number = Split(number);
						mode = 0;
						break;
				}
			}
			var mag = Magnitude(number);
			maxmag = maxmag > mag ? maxmag : mag;
		}

	}


	return maxmag;
}

private bool CanExplode(string number)
{
	var canExplode = false;

	//check the number for a series of 5 ['s in a row, it means that there is a pair that is nested 4 deep
	var openBracketStack = new Stack<char>();

	foreach (var c in number)
	{
		if (c == '[')
		{
			openBracketStack.Push('[');
		}
		if (c == ']')
		{
			openBracketStack.Pop();
		}
		if (openBracketStack.Count() > 4)
		{
			canExplode = true;
			break;
		}
	}

	return canExplode;
}

private int MaxExplodeBraceIndex(string number)
{
	var openBracketStack = new Stack<char>();
	var maxOpenBraces = 0;
	var index = 0;


	//get index of opening brace
	for (int i = 0; i < number.Length; i++)
	{
		if (number[i] == '[')
		{
			openBracketStack.Push('[');
			if (openBracketStack.Count() > 4)
			{
				return i;
			}
		}
		if (number[i] == ']')
		{
			openBracketStack.Pop();
		}
	}

	return index;
}

private bool CanSplit(string number)
{
	//look for any two digit numbers to split
	var rx = new Regex("[0-9]{2}");

	var m = rx.Match(number);

	return m.Success;
}

private string Explode(string number)
{
	//get index of opening brace
	int index = MaxExplodeBraceIndex(number);

	var exNum = "";

	for (int i = index; i < 100000; i++)
	{
		exNum += number[i];
		if (number[i] == ']')
		{
			break;
		}
	}

	var leftnum = int.Parse(exNum.Split(',')[0].Substring(1));
	var rightnum = int.Parse(exNum.Split(',')[1][..^1]);

	var rx = new Regex("[0-9]+");
	//find the first number to the right
	var rightindex = index + 5;
	if (rightnum > 9)
	{
		rightindex++;
	}
	var match = rx.Match(number[rightindex..]);
	if (match.Success)
	{
		var newRight = rightnum + int.Parse(match.Value);
		number = number.Remove(rightindex + match.Index, match.Value.Length);
		number = number.Insert(rightindex + match.Index, newRight.ToString());
	}

	//replace original number with 0
	number = number.Remove(index, exNum.Length);
	number = number.Insert(index, "0");

	//find the first number to the left
	var matches = rx.Matches(number[..index]);
	if (matches.Any())
	{
		var m = matches.Last();

		var newLeft = leftnum + int.Parse(m.Value);
		number = number.Remove(m.Index, m.Value.Length);
		number = number.Insert(m.Index, newLeft.ToString());
	}
	return number;
}

private string Split(string number)
{
	var rx = new Regex("[0-9]{2}");

	var m = rx.Match(number);

	var split = $"[{Math.Floor(double.Parse(m.Value) / 2)},{Math.Ceiling(double.Parse(m.Value) / 2)}]";

	number = number.Remove(m.Index, m.Value.Length);
	number = number.Insert(m.Index, split);

	return number;
}

private ulong Magnitude(string number)
{
	var rx = new Regex("\\[[0-9]+,[0-9]+\\]");

	while (number.Contains('['))
	{
		var m = rx.Match(number);

		var leftnum = int.Parse(m.Value.Split(',')[0].Substring(1));
		var rightnum = int.Parse(m.Value.Split(',')[1][..^1]);

		var mag = ((leftnum * 3) + (rightnum * 2)).ToString();

		number = number.Replace(m.Value, mag);
	}

	return ulong.Parse(number);
}
