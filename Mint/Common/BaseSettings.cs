using System.Diagnostics;

using LibBSP;

using Spectre.Console.Cli;

namespace Mint.Common;

public class BaseSettings : CommandSettings
{
	[CommandOption("-f|--file <FILE>")]
	public string File { get; set; }

	public FileInfo GetInfo()
	{
		return new FileInfo(File + ".bsp");
	}

	public BSPReader Read()
	{
		return new BSPReader(GetInfo());
	}

	public (BSP, Stopwatch) Full()
	{
		Stopwatch s = Stopwatch.StartNew();

		BSP map = new BSP(GetInfo());

		return (map, s);
	}
}