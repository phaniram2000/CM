using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;

public class DragWireInputHandler : AInputHandler
{
    protected override void InitialiseDerivedState()
    {
        
    }

    protected override InputStateBase HandleInput()
    {
        if (HasNoInput()) return CurrentInputState;
        var ray = Camera.ScreenPointToRay(InputExtensions.GetInputPosition());
        if (!Physics.Raycast(ray, out var hit, InputStateBase.RaycastDistance)) return CurrentInputState;

        if (!hit.collider.TryGetComponent(out WireEndCube cube)) return CurrentInputState;

        return new DragWireState(cube);
    }
}
