using System.Collections.Generic;
using StateMachine;
using UnityEngine;

public sealed class PatternRuleSet : MonoBehaviour, IRuleSet
{
	[SerializeField] private int minGridItemsTraversed;
	public HelperBase GetHelperBase => null;

	public bool CanResetInput => true;

	public int CheckGameResult(int input)
	{
		if (DoesMeetUnderflowCondition(input)) return -1;
		if (DoesMeetOverflowCondition(input)) return 1;

		return 0;
	}

	public bool DoesMeetUnderflowCondition(int input) => input < minGridItemsTraversed;

	public bool DoesMeetOverflowCondition(int input) =>
		!IsCorrectPattern(DrawPatternInputHandler.DrawPatternMechanic.GetCurrentPattern());

	private bool IsCorrectPattern(List<Collider> traversed) => minGridItemsTraversed == traversed.Count;

	public bool TryResetInput()
	{
		if (!DrawPatternInputHandler.DrawPatternMechanic) return false;
		
		DrawPatternInputHandler.DrawPatternMechanic.ClearPattern();
		return true;
	}
}