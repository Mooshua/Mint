using LibBSP;

using Mint.Common;

using Spectre.Console;

namespace Mint.Scripting;

public class LuaEntity
{

	protected Entity Source { get; set; }

	/// <summary>
	///	A list of events 
	/// </summary>
	public List<LuaEvent> Events { get; protected set; } = new List<LuaEvent>();

	public Dictionary<string, string> Values { get; protected set; } = new Dictionary<string, string>();
	
	public Prefix? Target { get; set; } = null;

	internal void Commit()
	{
		
		FancyConsole.Write("Lua:Patch", $"Committing entity {Markup.Escape(Target is null ? "unnamed" : Target.ToString() )}");


		{
			var removed = Source.connections.ConvertAll<LuaEvent>((input => LuaEvent.From(input))).Except(Events);
				
			var added = Events.Except(Source.connections.ConvertAll(input => LuaEvent.From(input)));

			foreach (LuaEvent luaEvent in removed)
			{
				FancyConsole.Write("Lua:Patch", $"[red]-[/] [maroon]{luaEvent.ToStringFancy()}[/]");
			}
			
			foreach (LuaEvent luaEvent in added)
			{
				FancyConsole.Write("Lua:Patch", $"[lime]+[/] [lime]{luaEvent.ToStringFancy()}[/]");
			}
		}
		
		Source.connections.Clear();
		foreach (LuaEvent luaEvent in Events)
		{
			Source.connections.Add(luaEvent.Serialize());
		}
		Source.Clear();
		foreach (var (key, value) in Values)
		{
			if (key == "model" && (value == string.Empty || value == null))
				continue;
			
			Source[key] = value;
		}

		if (Target is not null)
			Source.Name = Target.ToString();
	}

	public LuaEntity SetKey(string k, string v)
	{
		Values[k] = v;
		return this;
	}
	
	public LuaEntity SetKey(string k, double v)
	{
		Values[k] = v.ToString();
		return this;
	}

	public LuaEntity SetTarget(Prefix v)
	{
		Target = v;
		return this;
	}

	public LuaEntity AddEvent(LuaEvent ev)
	{
		Events.Add(ev);
		return this;
	}

	internal LuaEntity(Entity source)
	{
		Source = source;

		foreach (var (key, value) in Source)
		{

			//var ev = LuaEvent.From(key, value);
			//if (ev is not null)
				//Events.Add(ev);
			//else
				Values.Add(key, value);

			if (key == "targetname")
				Target = new Prefix(value);
			

		}
		foreach (Entity.EntityConnection sourceConnection in Source.connections)
		{
			Events.Add( LuaEvent.From(sourceConnection) );
		}

	}
}