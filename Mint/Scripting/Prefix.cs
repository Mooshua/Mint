using System.Security.Cryptography;

namespace Mint.Scripting;

/// <summary>
/// A prefix describes the targetname of an object
/// & enables multicasting
/// </summary>
public class Prefix
{
	
	public Prefix? Parent { get; private set; }
	
	public String Value { get; private set; }

	public Prefix Extend(Prefix child)
	{

		return new Prefix()
		{
			Parent = this,
			Value = child
		};

	}
	
	public Prefix Extend(string child)
	{

		return new Prefix()
		{
			Parent = this,
			Value = child
		};

	}

	public Prefix Wildcard()
	{
		return new Prefix()
		{
			Parent = this,
			Value = "*"
		};
	}

	public Prefix(string name)
	{
		Parent = null;
		Value = name;
	}

	public Prefix()
	{
		Parent = null;
		Value = string.Empty;
	}

	public override string ToString()
	{
		if (Parent is not null)
		{
			return Parent.ToString() + Value;
		}

		return Value;
	}

	#region Static Utilities

	/// <summary>
	/// An integer all but guaranteed to be unique
	/// </summary>
	protected static int UniqueInt = 0;

	/// <summary>
	/// Create a new unique prefix which is basically guaranteed to be unique
	/// </summary>
	/// <returns></returns>
	public Prefix Unique()
	{
		UniqueInt+= 99;

		var prefix = new Prefix()
		{
			Parent = null,
			Value = Convert.ToHexString( SHA256.HashData(BitConverter.GetBytes(UniqueInt)) ).Substring(0,12)
		};

		return prefix;
	}

	#endregion
	
	#region Conversion

	public static implicit operator string(Prefix value) => value.ToString();
	public static implicit operator Prefix(string value) => new Prefix(value);


	#endregion
}