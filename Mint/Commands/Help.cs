using Spectre.Console;
using Spectre.Console.Cli;

namespace Mint.Commands;

public class HelpCommand : Command<HelpCommand.HelpSettings>
{
	public class HelpSettings : CommandSettings
	{
		
	}

	public override int Execute(CommandContext context, HelpSettings settings)
	{
		AnsiConsole.WriteLine("Hi!");
		return 0;
	}
}