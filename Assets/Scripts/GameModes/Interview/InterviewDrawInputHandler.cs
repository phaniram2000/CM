using StateMachine;
using UnityEngine;

public class InterviewDrawInputHandler : AInputHandler
{
    public static InterviewDrawmechanic DrawMechanic { get; set; }
    //drawing
    private static InterviewDrawState _drawState;
		
    protected override void InitialiseDerivedState()
    {
        DrawMechanic = GetComponentInChildren<InterviewDrawmechanic>();
        _drawState = new InterviewDrawState(DrawMechanic);
    }

    protected override InputStateBase HandleInput()
    {
        if (HasNoInput()) return CurrentInputState;

        var ray = Camera.ScreenPointToRay(InputExtensions.GetInputPosition());
        if (!Physics.Raycast(ray, out var hit, InputStateBase.RaycastDistance)) return CurrentInputState;
			
        if(hit.collider.CompareTag("DrawArea")) return _drawState;
			
        return CurrentInputState;
    }
}
