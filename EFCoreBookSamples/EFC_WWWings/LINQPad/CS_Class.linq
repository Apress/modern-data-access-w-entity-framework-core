<Query Kind="Statements" />

var e = new Result() { a = 1, b = 20 };

for (int i = 0; i < e.b; i++)
{
	e.a += i;
	Console.WriteLine(i + ";" + e.a);
}
} // This extra parenthesis is required!

// The type definition must be after the main program!
class Result
{
	public int a { get; set; }
	public int b { get; set; }
// // here you have to omit the parenthesis!
