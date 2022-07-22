using System.Diagnostics;

using Spectre.Console;

namespace Mint.Common;

public static class FancyConsole
{
	private const int LEFT_LENGTH = 15;

	public static void Write(string left, string right)
	{
		AnsiConsole.MarkupLine("{2}[bold olive]{0}[/]  [grey]{1}[/]", left, right, new string(' ', 15 - left.Length));
	}

	public static void WriteHappy(string left, string right, Stopwatch? time = null)
	{
		if (time is not null)
		{
			AnsiConsole.MarkupLine("{2}[bold teal]{0}[/]  [bold green]({3}ms)[/] {1}", left, right, new string(' ', 15 - left.Length), time.ElapsedMilliseconds);
			return;
		}
		AnsiConsole.MarkupLine("{2}[bold teal]{0}[/]  {1}", left, right, new string(' ', 15 - left.Length));
	}
	
	public static void WriteAngy(string left, string right)
	{
		AnsiConsole.MarkupLine("{2}[bold purple]{0}[/]  {1}", left, right, new string(' ', 15 - left.Length));
	}
	
	public static void WriteFatal(string left, string right)
	{
		AnsiConsole.MarkupLine("{2}[bold red]{0}[/]  ⚠ ERROR: {1}", left, right, new string(' ', 15 - left.Length));
	}
}