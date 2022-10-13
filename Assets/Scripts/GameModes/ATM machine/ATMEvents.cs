using System;

public static partial class ATMEvents 
{
    public static event Action GirlCanPeak;

    public static event Action SwitchToDebitInCam,SwitchToStealCam;

    public static event Action AllowPlayerToSteal,PlayerAtemptToSteal;

    public static event Action StealSuccess, StealFail;

    public static event Action GirlFall,MakeGirlMoveTowardsATM;

    public static event Action EnterAtmPinGamePlay;

    public static event Action RightAnswerPressed, WorngAnswerPressed;

    public static event Action EnableMoneySlider;

    public static event Action WithDrawlButtonPressed;

    public static event Action DisableBoysPocketAtmCard;

}


public static partial class ATMEvents 
{
    public static void InvokeOnGirlCanPeak() => GirlCanPeak?.Invoke();

    public static void InvokeOnSwitchToDebitInCam() => SwitchToDebitInCam?.Invoke();

    public static void InvokeOnSwitchToStealCam() => SwitchToStealCam?.Invoke();

    public static void InvokeOnAllowPlayerToSteal() => AllowPlayerToSteal?.Invoke();

    public static void InvokeOnPlayerAtemptToSteal() => PlayerAtemptToSteal?.Invoke();

    public static void InvokeOnStealSuccess() => StealSuccess?.Invoke();

    public static void InvokeOnStealFail() => StealFail?.Invoke();

    public static void InvokeOnGirlFall() => GirlFall?.Invoke();

    public static void InvokeOnMakeGirlMoveTowardsAtm() => MakeGirlMoveTowardsATM?.Invoke();

    public static void InvokeOnEnterAtmPinGamePlay() => EnterAtmPinGamePlay?.Invoke();

    public static void InvokeOnRightAnswerPressed() => RightAnswerPressed?.Invoke();

    public static void InvokeOnWorngAnswerPressed() => WorngAnswerPressed?.Invoke();

    public static void InvokeOnEnableMoneySlider() => EnableMoneySlider?.Invoke();

    public static void InvokeOnWithDrawlButtonPressed() => WithDrawlButtonPressed?.Invoke();

    public static void InvokeOnDisableBoysPocketAtmCard() => DisableBoysPocketAtmCard?.Invoke();
}