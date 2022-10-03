using System.Collections.Generic;

using LibBSP;

namespace Mint.Scripting;

public class LuaEntityList : List<LuaEntity>
{
	protected static Entities Source { get; set; }
	
	protected static LuaEntityList Instance { get; set; }

	public LuaEntityList(Entities source)
	{
		Source = source;
		Instance = this;
	}
	
	public LuaEntity New(string classname)
	{
		var ent = new Entity() { ClassName = classname };
		Source.Add(ent);

		var lua = new LuaEntity(ent)
		{
			Target = new Prefix().Unique()
		};
		Instance.Add(lua);

		return lua;
	}

	public LuaEntity? Find(Prefix pre)
	{
		foreach (LuaEntity entity in Instance)
		{
			if (entity.Target is not null)
				if (entity.Target.ToString() == pre.ToString())
					return entity;
		}
		return null;
	}
	
	public LuaEntity? Find(string pre)
	{
		return Find(new Prefix(pre));
	}
	
}