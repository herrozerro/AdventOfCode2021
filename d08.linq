<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

#load "utilities.linq"

async Task Main()
{
	//testing
	Debug.WriteLine("Beginning Tests");
	Debug.Assert(await p1(true) == 26);
	Debug.Assert(await p2(true) == 61229);
	Debug.WriteLine("Tests Complete");

	await p1().Dump();
	await p2().Dump();
}

// You can define other methods, fields, classes and namespaces here
public async Task<int> p1(bool test = false)
{
	var lines = await GetLinesFromFile("d08", test);
	
	var inst = 0;
	
	foreach (var line in lines)
	{
		inst += line.Split('|').Last().Split(' ').Count(l => l.Length == 2 || l.Length == 3 || l.Length == 4 || l.Length == 7);
	}
	
	return inst;
}

public async Task<int> p2(bool test = false)
{
	var lines = await GetLinesFromFile("d08", test);
	
	var total = 0;
	
	foreach (var line in lines)
	{
		var input = line.Split('|')[0].Split(" ").Where(l => l.Length > 0).ToList();
		var segmentmap = ParseSegments(input);
		var number = "";
		foreach (var character in line.Split('|')[1].Split(" ").Where(l => l.Length > 0).ToList())
		{
			number += segmentmap[new string(character.OrderBy(o => o).ToArray())];
		}
		total += int.Parse(number);
	}
	
	return total;
}

private Dictionary<string, string> ParseSegments(List<string> segments)
{
	//  0:      1:      2:      3:      4:
	// aaaa    ....    aaaa    aaaa    ....
	//b    c  .    c  .    c  .    c  b    c
	//b    c  .    c  .    c  .    c  b    c
	// ....    ....    dddd    dddd    dddd
	//e    f  .    f  e    .  .    f  .    f
	//e    f  .    f  e    .  .    f  .    f
	// gggg    ....    gggg    gggg    ....
	//  5:      6:      7:      8:      9:
	// aaaa    aaaa    aaaa    aaaa    aaaa
	//b    .  b    .  .    c  b    c  b    c
	//b    .  b    .  .    c  b    c  b    c
	// dddd    dddd    ....    dddd    dddd
	//.    f  e    f  .    f  e    f  .    f
	//.    f  e    f  .    f  e    f  .    f
	// gggg    gggg    ....    gggg    gggg

	//  5:    
	// dddd   
	//e    a  
	//e    a 
	// ffff   
	//g    b  
	//g    b  
	// cccc   



	//acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab

	// ab		1
	// dab		7
	// eafb		4
	// cdfbe	
	// gcdfa	
	// fbcad	3
	// cagedb	0
	// cefabd	9
	// cdfgeb   6
	// acedgfb	8

	// 1 is the only number with two segments so
	// // a|b == c|f
	// of the 6 digit displays only 6 doesn't have a/b in it, so that means that cdfgeb = 6
	// 6 == cdfgeb
	// because a is missing from 6, a is c
	// a == c
	// because d is new in dba
	// d == a
	// because 4 has d and 0 does not. eafb - 4  means that cagedb = 0 because it's missing f from the remaining 6 segment displays
	// f = d
	// because 0 and 9 are different, their differences are f and g, f is d so g is e cagedb and cefabd
	// g = e
	// 3 is the only 5 segment with 1 in it so cdfbe is 3
	// 

	// 1. take 2 and 3 length, difference is a
	// 2. take all 6 segment, any missing from 2 segment is 6.  Missing segment is c
	// 3. take remaining 6 segments, compare with 4 segment.  missing segment is d, 6 segment missing d is 0, other is 9
	// 4. all 6 segments are complete
	// 5. compare 0 and 9, what's missing from 9 what's not d is e
	var a = ' ';
	var b = ' ';
	var c = ' ';
	var d = ' ';
	var e = ' ';
	var f = ' ';
	var g = ' ';

	var one = segments.First(s => s.Length == 2); ;
	var two = "";
	var three = "";
	var four = segments.First(s => s.Length == 4);
	var five = "";
	var six = "";
	var seven = segments.First(s => s.Length == 3);
	var eight = segments.First(s => s.Length == 7);
	var nine = "";
	var zero = "";

	var sixsegs = segments.Where(s => s.Length == 6).ToList();
	var fivesegs = segments.Where(s => s.Length == 5).ToList();

	// 1. take 2 and 3 length, difference is a

	foreach (var character in seven)
	{
		if (!one.Contains(character))
			a = character;
	}

	// 2. take all 6 segment, any missing from one segment is sic.  Missing segment is c
	foreach (var sixseg in sixsegs)
	{
		foreach (var character in one)
		{
			if (!sixseg.Contains(character))
			{
				six = sixseg;
				c = character;
			}
		}
	}
	sixsegs.Remove(six);

	// 3. take remaining 6 segments, compare with 4 segment.  missing segment is d, 6 segment missing d is 0, other is 9
	foreach (var sixseg in sixsegs)
	{
		foreach (var character in four)
		{
			if (!sixseg.Contains(character))
			{
				zero = sixseg;
				d = character;
			}
		}
	}
	nine = sixsegs.First(s => s != zero);
	
	
	// 3 is the only 5 segment with 1 in it
	three = fivesegs.First(fi => fi.Contains(one[0]) && fi.Contains(one[1]));
	fivesegs.Remove(three);

	//Five is missing c so two remains
	two = fivesegs.First(fi => fi.Contains(c));
	five = fivesegs.First(fi => fi != two);
	
	var segmentmap = new Dictionary<string, string>();
	
	segmentmap.Add(new string(one.OrderBy(o => o).ToArray()),"1");
	segmentmap.Add(new string(two.OrderBy(o => o).ToArray()),"2");
	segmentmap.Add(new string(three.OrderBy(o => o).ToArray()),"3");
	segmentmap.Add(new string(four.OrderBy(o => o).ToArray()),"4");
	segmentmap.Add(new string(five.OrderBy(o => o).ToArray()),"5");
	segmentmap.Add(new string(six.OrderBy(o => o).ToArray()),"6");
	segmentmap.Add(new string(seven.OrderBy(o => o).ToArray()),"7");
	segmentmap.Add(new string(eight.OrderBy(o => o).ToArray()),"8");
	segmentmap.Add(new string(nine.OrderBy(o => o).ToArray()),"9");
	segmentmap.Add(new string(zero.OrderBy(o => o).ToArray()),"0");
	
	return segmentmap;
}