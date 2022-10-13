using StateMachine;
using UnityEngine;

public class BankRuleSet : MonoBehaviour, IRuleSet
{
	public int desiredGestureId = 80085;
	[HideInInspector] public BankHelper helper;

	public HelperBase GetHelperBase => helper;

	public bool CanResetInput => true;

	private void Awake() => helper = GetComponent<BankHelper>();

	public int CheckGameResult(int input)
	{
		return DoesMeetOverflowCondition(input) ? 1 : 0;
	}

	public bool DoesMeetUnderflowCondition(int input) => false;

	public bool DoesMeetOverflowCondition(int input) => input != desiredGestureId;

	public bool TryResetInput()
	{
		if (!DrawInputHandler.DrawMechanic) return false;
		
		DrawInputHandler.DrawMechanic.ClearDrawnLines();
		return true;
	}
}