using System.Diagnostics;

using LibBSP;

using Mint.Common;
using Mint.States;

using Spectre.Console;
using Spectre.Console.Cli;

namespace Mint.Commands;

public class RevertCommand: Command<RevertCommand.RevertSettings>
{

	public class RevertSettings : BaseSettings
	{

	}

	public override int Execute(CommandContext context, RevertSettings settings)
	{
		Stopwatch full = Stopwatch.StartNew();
		AnsiConsole.Status()
			.Spinner(Spinner.Known.Dots2)
			.Start("\nReverting...", ctx =>
			{

				FancyConsole.Write("Command", $"Attempting to revert {settings.GetInfo().Name}");
				
				(BSP level, Stopwatch levelLoadTime) = settings.Full();
				FancyConsole.WriteHappy("Command","Loaded BSP", levelLoadTime);

				new RevertState() { Mutable = level }.Execute();

			});
		
		return 0;
	}
}