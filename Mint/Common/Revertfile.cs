using MessagePack;
using LibBSP;

namespace Mint.Common;

/// <summary>
/// Contains a list of entities as they were before the preprocessor executed
/// Before any execution of the lua context is performed, the map is restored to Revertfile state
/// </summary>
[MessagePackObject()]
public class Revertfile
{

	/// <summary>
	/// The original entity lump
	/// </summary>
	[Key("ent")]
	public byte[]? Entities { get; set; }

	/// <summary>
	/// The original model lump
	/// </summary>
	[Key("mod")]
	public byte[]? Models { get; set; }

}