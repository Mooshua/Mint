using LibBSP;

namespace Mint.Common;

public abstract class State
{
	public BSP Mutable { get; set; }

	public abstract void Execute();

	/// <summary>
	/// Used to chain together states in commands
	/// Executes the current state then returns the arg
	/// </summary>
	/// <param name="next"></param>
	/// <returns></returns>
	public State Chain(State next)
	{
		Execute();

		next.Mutable = Mutable;

		return next;
	}
}