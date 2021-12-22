<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

#load "utilities.linq"

async Task Main()
{
	//testing
	Debug.WriteLine("Beginning Tests");
	Debug.Assert(await p1(true) == 739785);
	Debug.Assert(await p2(true) == 0);
	Debug.WriteLine("Tests Complete");

	await p1().Dump();
	await p2().Dump();
}

// You can define other methods, fields, classes and namespaces here
public async Task<int> p1(bool test = false)
{
	var lines = await GetLinesFromFile("d21", test);

	var p1Score = 0;
	var p2Score = 0;

	var p1Pos = int.Parse(lines[0][^1].ToString());
	var p2Pos = int.Parse(lines[1][^1].ToString());

	var rolls = 0;
	var diePos = 1;

	var pturn = 1;

	while (p1Score < 1000 && p2Score < 1000)
	{
		rolls += 3;
		
		var diepos2 = (diePos + 1) % 100 == 0 ? 100 : (diePos + 1) % 100;
		var diepos3 = (diePos + 2) % 100 == 0 ? 100 : (diePos + 2) % 100;
		var moves = diePos + diepos2 + diepos3;

		diePos = (diepos3 + 1) % 100;
		if (diePos == 0)
			diePos = 100;

		switch (pturn)
		{
			case 1:
				p1Pos = (p1Pos + moves) % 10;
				if (p1Pos == 0)
					p1Pos = 10;
				p1Score += p1Pos;
				break;
			case 2:
				p2Pos = (p2Pos + moves) % 10;
				if (p2Pos == 0)
					p2Pos = 10;
				p2Score += p2Pos;
				break;
		}
		
		if (pturn == 1)
		{
			pturn = 2;
		}
		else
		{
			pturn = 1;
		}
	}


	return rolls * Math.Min(p1Score, p2Score);
}

public async Task<int> p2(bool test = false)
{
	var lines = await GetLinesFromFile("d21", test);

	return 0;
}

