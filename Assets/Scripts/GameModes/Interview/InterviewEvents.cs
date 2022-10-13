using System;


public static partial class InterviewEvents
{
    public static event Action MoveCheckToPlayer;

    public static event Action SwitchToOfferCam;

    public static event Action BossRotationDone;

    public static event Action SignDone,DoneButtonPressed;

    public static event Action BonusGiven;
}

public static partial class InterviewEvents 
{
    public static void InvokeOnMoveCheckToPlayer() => MoveCheckToPlayer?.Invoke();

    public static void InvokeOnSwitchToSignCam() => SwitchToOfferCam?.Invoke();

    public static void InvokeOnBossRotationDone() => BossRotationDone?.Invoke();

    public static void InvokeOnSignDone() => SignDone?.Invoke();

    public static void InvokeOnDoneButtonPressed() => DoneButtonPressed?.Invoke();

    public static void InvokeOnBonusGiven() => BonusGiven?.Invoke();
}
