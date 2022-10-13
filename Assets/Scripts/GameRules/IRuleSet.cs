public interface IRuleSet
{
	public virtual HelperBase GetHelperBase => null;

	public bool CanResetInput { get; }

	/// <summary>
	/// Check game result
	/// </summary>
	/// <param name="input"></param>
	/// <returns> <para>Returns -1 if lost because didn't meet passing condition.</para>
	/// <para>0 if won.</para>
	/// <para>1 if you lost because you were overconfident.</para></returns>
	public int CheckGameResult(int input);
	
	/// <summary>
	/// Underconfident/ didnt't do enough to win the game
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	public bool DoesMeetUnderflowCondition(int input);
	
	/// <summary>
	/// overconfident/ did too much to win and got caught
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	public bool DoesMeetOverflowCondition(int input);

	/// <summary>
	/// Returns if was successful in clearing previously entered input
	/// </summary>
	public bool TryResetInput();
}