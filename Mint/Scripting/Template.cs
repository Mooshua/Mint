using LibBSP;

namespace Mint.Scripting;

/// <summary>
/// A model which can be re-used by entities
/// Possibly will someday come with features such as duplication or texture editing.
/// </summary>
public class Template
{
	public int Index { get; private set; }

	public static Template From(Entity ent)
	{

		return new Template
		{
			Index = ent.ModelNumber
		};

	}
}