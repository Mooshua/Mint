using System.Diagnostics;

using LibBSP;

using Mint.Common;

using NLua;

using Spectre.Console;

namespace Mint.Scripting;

public class LuaState : State
{

	protected Lua Engine { get; } = new Lua();

	public override void Execute()
	{
		//try {
			
			FancyConsole.Write("Task:Lua", "Starting lua state");
			Stopwatch s = Stopwatch.StartNew();
			LuaEntityList luaEntities = new LuaEntityList(Mutable.Entities);

			foreach (Entity mutableEntity in Mutable.Entities)
			{
				luaEntities.Add(new LuaEntity(mutableEntity));
			}

			Engine["Entities"] = luaEntities;
			Engine["Event"] = new LuaEvent();
			Engine["Prefix"] = new Prefix();

			//	Add the current map directory as a package
			( Engine["package"] as LuaTable )["path"] += $";{Mutable.Reader.BspFile.Directory.FullName}/?.lua";
			
			//	Attempt to make LuaEvent an autoconvert. Possibly works?
			Engine.RegisterLuaClassType(typeof(LuaEvent), typeof(LuaTable));

			//Engine.RegisterFunction("Prefix.Unique", typeof(Prefix).GetMethod("Unique"));
			//Engine.RegisterFunction("Entity.New", typeof(LuaEntityList).GetMethod("New"));

			
/*
			Engine.HookException += (sender, args) =>
			{
				FancyConsole.WriteAngy("Task:Lua", "Exception");
				AnsiConsole.WriteException(args.Exception);
			};
*/
			FancyConsole.Write("Task:Lua","Loading lua");
			
			var func = Engine.LoadFile(Mutable.Reader.BspFile.FullName.Substring(0, Mutable.Reader.BspFile.FullName.Length - 4) + ".lua");
			
			FancyConsole.Write("Task:Lua","Executing lua");

			func.Call();
			
			FancyConsole.WriteHappy("Task:Lua", "Finished executing lua scripts!", s);

			foreach (LuaEntity entity in luaEntities)
			{
				entity.Commit();
			}
		//}
		/*atch (Exception e)
		{
			FancyConsole.WriteAngy("Task:Lua", "Exception");
			throw e;
			
			
		}*/
	}
}