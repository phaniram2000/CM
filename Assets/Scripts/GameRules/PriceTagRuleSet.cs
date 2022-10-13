using UnityEngine;

public class PriceTagRuleSet : MonoBehaviour, IRuleSet
{
    [SerializeField] private int requiredPrice = 1;
    public bool CanResetInput { get; }
    public int CheckGameResult(int input)
    {
        if (DoesMeetUnderflowCondition(input)) return -1;
        if (DoesMeetOverflowCondition(input)) return 1;

  
        return 0;
    }

    public bool DoesMeetUnderflowCondition(int input) => input != requiredPrice;

    public bool DoesMeetOverflowCondition(int input) => input != requiredPrice;
    
    public bool TryResetInput()
    {
        if (!EraseDrawInputHandler.DrawMechanic) return false;
		
        EraseDrawInputHandler.DrawMechanic.ClearDrawnLines();
        return true;
    }
}
