using System.Diagnostics;

using LibBSP;

using Mint.Common;
using Mint.Scripting;
using Mint.States;

using Spectre.Console;
using Spectre.Console.Cli;

namespace Mint.Commands;

public class TransformCommand : Command<TransformCommand.TransformSettings>
{

	public class TransformSettings : BaseSettings
	{

	}

	public override int Execute(CommandContext context, TransformSettings settings)
	{
		Stopwatch full = Stopwatch.StartNew();
		AnsiConsole.Status()
			.Spinner(Spinner.Known.Dots2)
			.Start("\nReverting...", ctx =>
			{

				FancyConsole.Write("Command", $"Attempting to transform {settings.GetInfo().Name}");
				
				(BSP level, Stopwatch levelLoadTime) = settings.Full();
				FancyConsole.WriteHappy("Command","Loaded BSP", levelLoadTime);

				new RevertState() { Mutable = level, IgnoreMissingRevertfile = true }
					.Chain(new PreserveState())
					.Chain(new LuaState())
					.Chain(new WriteState())
					.Execute();

			});
		
		return 0;
	}
}