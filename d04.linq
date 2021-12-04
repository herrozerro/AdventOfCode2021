<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

#load "utilities.linq"

async Task Main()
{
	//testing
	Debug.WriteLine("Beginning Tests");
	Debug.Assert(await p1(true) == 4512);
	Debug.Assert(await p2(true) == 1924);
	Debug.WriteLine("Tests Complete");

	await p1().Dump();
	await p2().Dump();
}

// You can define other methods, fields, classes and namespaces here
public async Task<int> p1(bool test = false)
{
	var lines = await GetLinesFromFile("d04", test);
	
	var callList = lines.First().Split(',');
	
	var boards = new List<bingoboard>();
	
	for (int i = 0; i < lines.Skip(2).Count(); i += 6)
	{
		boards.Add(new bingoboard(lines.Skip(2+i).Take(5).ToList()));
	}
	
	foreach (var call in callList)
	{
		boards.ForEach(b => b.NumberCalled(call));
		
		if (boards.Any(x=>x.isWinner()))
		{
			return int.Parse(call) * boards.First(x => x.isWinner()).boardScore();
		}
	}
	
	return 4512;
}

public async Task<int> p2(bool test = false)
{
	var lines = await GetLinesFromFile("d04", test);

	var callList = lines.First().Split(',');

	var boards = new List<bingoboard>();

	for (int i = 0; i < lines.Skip(2).Count(); i += 6)
	{
		boards.Add(new bingoboard(lines.Skip(2 + i).Take(5).ToList()));
	}

	foreach (var call in callList)
	{
		boards.ForEach(b => b.NumberCalled(call));
		
		if (boards.Count() > 1)
		{
			if (boards.Any(x => x.isWinner()))
			{
				boards.RemoveAll(b => b.isWinner());
			}
		}
		else
		{
			if (boards.First().isWinner())
			{
				return int.Parse(call) * boards.First().boardScore();
			}
		}
	}

	return 1924;
}

public class bingoboard
{
	private KeyValuePair<string, bool>[,] board = new KeyValuePair<string, bool>[5, 5];

	public bingoboard(List<string> boardData)
	{
		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				board[i, j] = new KeyValuePair<string, bool>(new string(boardData[i].Skip((j * 2) + (j * 1)).Take(2).ToArray()), false);
			}
		}
	}

	public void NumberCalled(string number)
	{
		number = number.PadLeft(2);

		for (int i = 0; i < board.GetLength(0); i++)
		{
			for (int j = 0; j < board.GetLength(1); j++)
			{
				if (board[i, j].Key == number)
				{
					board[i, j] = new KeyValuePair<string, bool>(number, true);
				}
			}
		}
	}

	public bool isWinner()
	{
		bool isWinner = false;
		for (int i = 0; i < board.GetLength(0); i++)
		{
			isWinner = Enumerable.Range(0, board.GetLength(1))
				.Select(x => board[i, x]).All(x => x.Value);

			if (isWinner)
			{
				return true;
			}
		}

		for (int i = 0; i < board.GetLength(1); i++)
		{
			isWinner = Enumerable.Range(0, board.GetLength(0))
				.Select(x => board[x, i]).All(x => x.Value);

			if (isWinner)
			{
				return true;
			}
		}

		return isWinner;
	}

	public int boardScore()
	{
		int score = 0;

		for (int i = 0; i < board.GetLength(0); i++)
		{
			for (int j = 0; j < board.GetLength(1); j++)
			{
				if (!board[i, j].Value)
				{
					score += int.Parse(board[i, j].Key);
				}
			}
		}

		return score;
	}
}
