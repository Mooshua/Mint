// See https://aka.ms/new-console-template for more information
using LibBSP;

using Mint.Commands;
using Mint.Common;
using Mint.States;

using Spectre.Console;
using Spectre.Console.Cli;

var app = new CommandApp();

app.Configure(config =>
{
	//config.PropagateExceptions();
	config.SetExceptionHandler(exception =>
	{
		if (exception as SilentException is not null)
			return;
		
		AnsiConsole.WriteException(exception, ExceptionFormats.ShortenPaths);
	});
	config.SetApplicationName("Mint");
	config.AddCommand<RevertCommand>("revert")
		.WithAlias("r");
	config.AddCommand<PreserveCommand>("preserve")
		.WithAlias("p");
	config.AddCommand<TransformCommand>("transform")
		.WithAlias("t");
});

app.SetDefaultCommand<HelpCommand>();


return app.Run(args);