using StateMachine;
using UnityEngine;

public class PubRuleSet : MonoBehaviour, IRuleSet
{
	[SerializeField] private int minimumAge, maximumAge;
	public HelperBase GetHelperBase => null;

	public bool CanResetInput => true;

	public int CheckGameResult(int input)
	{
		if (DoesMeetUnderflowCondition(input)) return -1;
		if (DoesMeetOverflowCondition(input)) return 1;

		return 0;
	}

	public bool DoesMeetUnderflowCondition(int input) => input < minimumAge;

	public bool DoesMeetOverflowCondition(int input) => input > maximumAge;

	public bool TryResetInput()
	{
		if (!DrawInputHandler.DrawMechanic) return false;
		
		DrawInputHandler.DrawMechanic.ClearDrawnLines();
		return true;
	}
}