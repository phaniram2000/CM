using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;

public class TBRSwipeState : InputStateBase
{
    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void Execute()
    {
        base.Execute();
        print("in swipe execute.");
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
