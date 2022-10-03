using System.Diagnostics;
using System.IO;

using ICSharpCode.SharpZipLib.Zip;

using LibBSP;

using MessagePack;

using Mint.Common;
using Mint.Extensions;

namespace Mint.States;

/// <summary>
/// Preserve all lumps required by the Revertfile
/// </summary>
public class PreserveState : State
{
	public override void Execute()
	{
		
		Stopwatch time = Stopwatch.StartNew();

		var pak = Mutable[40];
		var zip = Mutable.Reader.ReadLump(pak);

		var paklump = new Pakfile(zip, Mutable, pak);
		
		FancyConsole.WriteHappy("Task:Preserve",$"Mounted pakfile", time);

		var entry = paklump.Get(RevertState.FILE_NAME);

		if (entry is not null)
		{
			paklump.Delete(entry);
		}

		var ms = new MemoryStream();
		MessagePackSerializer.Serialize<Revertfile>(ms,  new Revertfile()
		{
			Entities = Mutable.Entities.GetBytes(),
			Models = Mutable.Models.GetBytes(),
		});
		ms.Seek(0, SeekOrigin.Begin);

		paklump.Create( new Pakfile.StreamSource() { Source = ms }, new ZipEntry(RevertState.FILE_NAME));
		
		

		FancyConsole.WriteHappy("Task:Preserve", "Successfully preserved ENT and MODEL lumps in Revertfile.", time);
		
	}
}