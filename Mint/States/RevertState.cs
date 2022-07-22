using System.Diagnostics;

using ICSharpCode.SharpZipLib.Zip;

using LibBSP;

using MessagePack;

using Mint.Common;
using Mint.Extensions;

namespace Mint.States;

public class RevertState : State
{
	public const string FILE_NAME = "revert.mint";

	protected Revertfile? File;

	public bool IgnoreMissingRevertfile { get; set; }= false;
	
	public override void Execute()
	{
		Stopwatch time = Stopwatch.StartNew();

		var pak = Mutable[40];
		var zip = Mutable.Reader.ReadLump(pak);

		var paklump = new Pakfile(zip, Mutable, pak);
		
		FancyConsole.WriteHappy("Task:Revert",$"Mounted pakfile", time);

		var entry = paklump.Get(FILE_NAME);

		if (entry is null)
		{
			if (IgnoreMissingRevertfile)
				return;
			
			FancyConsole.WriteFatal("Task:Revert", "Unable to find Revertfile!");
			FancyConsole.WriteAngy("Task:Revert", "Are you sure this BSP has been transformed with Mint?");
			throw new SilentException("Unable to find revertfile");
		}

		try
		{

			var rvtfile = paklump.Read(entry);
			var snapshot = MessagePackSerializer.Deserialize<Revertfile>(rvtfile);

			FancyConsole.Write("Task:Revert", $"Found and decoded Revertfile");

			//	Dear god
			Mutable.Entities = new Entities(snapshot.Entities);
			Mutable.Models = Model.LumpFactory(snapshot.Models, Mutable, Mutable.Models.LumpInfo);

			FancyConsole.WriteHappy("Task:Revert", "Successfully reverted ENT and MODEL lumps from Revertfile", time);
		}
		catch (Exception e)
		{
			FancyConsole.WriteAngy("Task:Revert", "Failed to revert file: Deserialization error!");
			
			if (!IgnoreMissingRevertfile)
				throw new SilentException(e.Message);
		}
	}
}