using GestureRecognizer;
using StateMachine;
using UnityEngine;

public class EraseDrawInputHandler : AInputHandler
{
    public static DrawMechanic DrawMechanic { get; private set; }
    private static DrawState _drawState;

    protected override void OnEnable()
    {
        base.OnEnable();
        
        //subscrioptuion for erase state end
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        
    }

    protected override void InitialiseDerivedState()
    {
        DrawMechanic = GetComponentInChildren<DrawMechanic>();
        _drawState = new DrawState(DrawMechanic);
    }

    protected override InputStateBase HandleInput()
    {
        if (HasNoInput()) return CurrentInputState;
        
        var ray = Camera.ScreenPointToRay(InputExtensions.GetInputPosition());
        if (!Physics.Raycast(ray, out var hit, InputStateBase.RaycastDistance)) return CurrentInputState;

        //if !done with erase
        if (hit.collider.CompareTag("EraseTarget"))
            return new EraseState(hit.collider.GetComponent<EraseTarget>());
        else
        if (hit.collider.CompareTag("DrawArea")) 
            return _drawState;

        return CurrentInputState;
    }
}
