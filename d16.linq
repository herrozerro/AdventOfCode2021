<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

#load "utilities.linq"

static string solution = "";

async Task Main()
{
	//testing
	Debug.WriteLine("Beginning Tests");
	//Debug.Assert(await p1(true) == 12);
	//Debug.Assert(await p2(true) > 0);
	Debug.WriteLine("Tests Complete");

	await p1().Dump();
	await p2().Dump();
}

// You can define other methods, fields, classes and namespaces here
public async Task<int> p1(bool test = false)
{
	var line = await GetStringFromFile("d16", test);

	string binarystring = String.Join(String.Empty,
	  line.Select(
		c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')
	  )
	);

	var packets = ParsePacket(binarystring, 0);

	//packets.Dump();

	return (packets.packet.packets.Flatten(x => x.packets).Sum(x => x.version) + packets.packet.version);
}

public async Task<ulong> p2(bool test = false)
{
	var line = await GetStringFromFile("d16", test);

	string binarystring = String.Join(String.Empty,
	  line.Select(
		c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')
	  )
	);

	var packets = ParsePacket(binarystring, 0);

	packets.packet.Dump();

	var i = ProcessPackets(packets.packet);
	
	solution.Dump();
	return i;
}

private (packet packet, int cursorPos) ParsePacket(string bits, int cursorPos)
{
	var p = new packet();
	//get version
	p.version = Convert.ToInt32(string.Join("", bits.Skip(cursorPos).Take(3)), 2);
	cursorPos += 3;
	//get type
	p.type = Convert.ToInt32(string.Join("", bits.Skip(cursorPos).Take(3)), 2);
	cursorPos += 3;

	//literal value
	if (p.type == 4)
	{
		var isNotAtEnd = true;
		var literal = "";
		while (isNotAtEnd)
		{
			var slice = string.Join("", bits.Skip(cursorPos).Take(5));
			cursorPos += 5;
			if (slice.StartsWith("0"))
			{
				isNotAtEnd = false;
			}
			literal += slice[1..];
		}

		p.literalvalue = Convert.ToUInt64(literal, 2);
	}
	//operator packet
	else
	{
		p.mode = int.Parse(string.Join("", bits.Skip(cursorPos).Take(1)));
		cursorPos++;

		//subpackets are of a known length
		if (p.mode == 0)
		{
			var bitlen = Convert.ToInt32(string.Join("", bits.Skip(cursorPos).Take(15)), 2);
			cursorPos += 15;

			var slice = string.Join("", bits.Skip(cursorPos).Take(bitlen));

			while (slice.Length > 0)
			{
				var s = ParsePacket(slice, 0);
				p.packets.Add(s.packet);
				slice = string.Join("", slice.Skip(s.cursorPos));
			}



			cursorPos += bitlen;


		}
		else
		{
			var partCount = Convert.ToInt32(string.Join("", bits.Skip(cursorPos).Take(11)), 2);
			cursorPos += 11;
			for (int i = 0; i < partCount; i++)
			{
				var pak = ParsePacket(bits, cursorPos);
				p.packets.Add(pak.packet);
				cursorPos = pak.cursorPos;
			}
		}
	}

	return (p, cursorPos);
}


private UInt64 ProcessPackets(packet p)
{
	if (p.type != 4 && p.literalvalue == 0)
	{
		switch (p.type)
		{
			case 0: //sum
				solution += "(";
				p.packets.ForEach(pa => { ProcessPackets(pa); solution += " + "; });
				solution = solution[..^3];
				break;
			case 1: //product
				solution += "(";
				p.packets.ForEach(pa => { ProcessPackets(pa); solution += " * "; });
				solution = solution[..^3];
				break;
			case 2: //min
				solution += "Math.min(";
				p.packets.ForEach(pa => { ProcessPackets(pa); solution += ", "; });
				solution = solution[..^2];
				break;
			case 3: //max
				solution += "Math.max(";
				p.packets.ForEach(pa => { ProcessPackets(pa); solution += ", "; });
				solution = solution[..^2];
				break;
			case 5: //greater than
				solution += "(";
				p.packets.ForEach(pa => { ProcessPackets(pa); solution += " > "; });
				solution = solution[..^3];
				break;
			case 6: //less than
				solution += "(";
				p.packets.ForEach(pa => { ProcessPackets(pa); solution += " < "; });
				solution = solution[..^3];
				break;
			case 7: //equal
				solution += "(";
				p.packets.ForEach(pa => { ProcessPackets(pa); solution += " == "; });
				solution = solution[..^4];
				break;
		}

		solution += ")";
	}
	else
	{
		solution += $"{p.literalvalue}";
		return p.literalvalue;
	}
	
	return p.literalvalue;
}

public class packet
{
	public int version { get; set; }
	public int type { get; set; }
	public int mode { get; set; }
	public List<packet> packets { get; set; } = new List<packet>();
	public UInt64 literalvalue { get; set; } = 0;
}