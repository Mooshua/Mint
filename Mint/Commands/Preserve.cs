using System.Diagnostics;

using LibBSP;

using Mint.Common;
using Mint.States;

using Spectre.Console;
using Spectre.Console.Cli;

namespace Mint.Commands;

public class PreserveCommand: Command<PreserveCommand.PreserveSettings>
{

	public class PreserveSettings : BaseSettings
	{

	}

	public override int Execute(CommandContext context, PreserveSettings settings)
	{
		Stopwatch full = Stopwatch.StartNew();
		AnsiConsole.Status()
			.Spinner(Spinner.Known.Dots2)
			.Start("\nReverting...", ctx =>
			{

				FancyConsole.Write("Command", $"Attempting to preserve {settings.GetInfo().Name}");
				
				(BSP level, Stopwatch levelLoadTime) = settings.Full();
				FancyConsole.WriteHappy("Command","Loaded BSP", levelLoadTime);

				new PreserveState() { Mutable = level }
					.Chain(new WriteState())
					.Execute();

			});
		
		return 0;
	}
}