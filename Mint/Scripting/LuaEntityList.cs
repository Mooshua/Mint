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

		var lua = new LuaEntity(ent);
		Instance.Add(lua);

		return lua;
	}

	public LuaEntity? FirstWithPrefix(Prefix pre)
	{
		foreach (LuaEntity entity in Instance)
		{
			if (entity.Target is not null)
				if (entity.Target.ToString() == pre.ToString())
					return entity;
		}
		return null;
	}
	
	public LuaEntity? FirstWithPrefix(string pre)
	{
		return FirstWithPrefix(new Prefix(pre));
	}
	
}