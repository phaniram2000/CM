using StateMachine;
using UnityEngine;

public class ClassroomRuleSet : MonoBehaviour, IRuleSet
{
	public HelperBase GetHelperBase => null;

	public bool CanResetInput => true;

	public int CheckGameResult(int input)
	{
		if (DoesMeetUnderflowCondition(input)) return -1;
		if (DoesMeetOverflowCondition(input)) return 1;

		return 0;
	}

	[SerializeField] private int minimumMarks, maximumMarks;

	public bool DoesMeetUnderflowCondition(int input) => input < minimumMarks;

	public bool DoesMeetOverflowCondition(int input) => input > maximumMarks;

	public bool TryResetInput()
	{
		if (!DrawInputHandler.DrawMechanic) return false;
		
		DrawInputHandler.DrawMechanic.ClearDrawnLines();
		return true;
	}
}