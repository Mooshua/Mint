using System;
using System.Net.Mime;

using ICSharpCode.SharpZipLib.Zip;

using LibBSP;

using NLua;

namespace Mint.Scripting;

public class LuaEvent
{

	public static LuaEvent? From(string output, string source)
	{
		string[] components = source.Split(SEP_CHR_NEW);

		if (components.Length != 5)
		{
			components = source.Split(SEP_CHR_OLD);
			if (components.Length != 5)
			{
				return null;
			}
		}

		if (!float.TryParse(components[3], out var delay))
			return null;

		if (!int.TryParse(components[4], out var refire))
			return null;

		return new LuaEvent()
		{
			Output = output,
			Entity = new Prefix(components[0]),
			Input = components[1],
			Override = components[2],
			Delay = delay,
			Refire = refire,
		};
	}

	internal static LuaEvent From(Entity.EntityConnection connection)
	{
		return new LuaEvent
		{
			Delay = connection.delay,
			Refire = connection.fireOnce,
			Entity = connection.target,
			Input = connection.action,
			Output = connection.name,
			Override = connection.param,
		};
	}
	
	public const char SEP_CHR_OLD = ',';
	public const char SEP_CHR_NEW = (char)0x1b;
	
	/// <summary>
	/// The name of the output which summons this event
	/// </summary>
	public string Output { get; set; }
	
	/// <summary>
	/// The entities this event effects
	/// </summary>
	public Prefix Entity { get; set; }
	
	public string Input { get; set; }
	
	public string Override { get; set; } = String.Empty;

	public float Delay { get; set; } = 0;

	public int Refire { get; set; } = -1;

	public override string ToString()
	{
		return $"{Entity}{SEP_CHR_NEW}{Input}{SEP_CHR_NEW}{Override}{SEP_CHR_NEW}{Delay}{SEP_CHR_NEW}{Refire}";
	}
	
	public string ToStringFancy()
	{
		return $"(On {Output}): {Entity}->{Input} ({Override}) [[d {Delay}; r {Refire}]]";
	}
	
	public LuaEvent New()
	{
		return new LuaEvent();
	}
	
	public LuaEvent New(string output, Prefix entity, string input, string param = null, float delay = 0, int refire = -1)
	{
		return new LuaEvent()
		{
			Output = output,
			Entity = entity,
			Input = input,
			Override = param,
			Delay = delay,
			Refire = refire
		};
	}

	internal Entity.EntityConnection Serialize()
	{
		return new Entity.EntityConnection
		{
			name = Output,
			target = Entity.ToString(),
			action = Input,
			param = Override,
			delay = Delay,
			fireOnce = Refire,
			unknown0 = null,
			unknown1 = null
		};
	}

	public static LuaEvent Table(LuaTable t)
	{
		float delay = 0;
		int refire = 0;

		var delaySuccessful = t["Delay"] is not null ? float.TryParse(t["Delay"].ToString(), out delay) : false;
		var refireSuccessful = t["Refire"] is not null ? int.TryParse(t["Refire"].ToString(), out refire) : false;

		
		
		var ev = new LuaEvent()
		{
			Output = t["Output"].ToString() ?? throw new InvalidOperationException(),
			Entity = t["Entity"].ToString() ?? throw new InvalidOperationException(),
			Input = t["Input"].ToString() ?? throw new InvalidOperationException(),
			Override = (t["Param"] is not null ? t["Param"].ToString() : string.Empty),
			Delay = (delaySuccessful ? delay : 0),
			Refire = (refireSuccessful ? refire : -1),
		};

		return ev;
	}
	
	public static implicit operator LuaEvent(LuaTable t)
	{
		return Table(t);

	}
}