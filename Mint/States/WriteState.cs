using System.Diagnostics;

using LibBSP;

using Mint.Common;

namespace Mint.States;

public class WriteState : State
{
	public override void Execute()
	{
		Stopwatch time = Stopwatch.StartNew();
		new BSPWriter(Mutable)
		{
		}.WriteBSP(Mutable.Reader.BspFile.FullName);
		FancyConsole.WriteHappy("Task:Write", "Successfully wrote BSP to disk!", time);
		FancyConsole.Write("Task:Write",$"BSP location: {Mutable.Reader.BspFile.FullName}");
	}
}