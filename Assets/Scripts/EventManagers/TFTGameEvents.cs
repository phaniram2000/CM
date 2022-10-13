using System;


public static partial class TFTGameEvents
{
    public static event Action ActivateNextScene;

    public static event Action DoSceneTransition,OpenLiftDoors,CloseLiftDoors;

    public static event Action<float> LiftOpenDoorsDone;

    public static event Action ShowGameplayScreen;

    public static event Action AllButtonsPressed;

    public static event Action DoneButtonPressed;


}
public static partial class TFTGameEvents
{

    public static void InvokeOnActivateNextScene() => ActivateNextScene?.Invoke();

    public static void InvokeOnDoSceneTransition() => DoSceneTransition?.Invoke();

    public static void InvokeOnOpenLiftDoors() => OpenLiftDoors?.Invoke();

    public static void InvokeOnLiftOpenDoorsDone(float obj) => LiftOpenDoorsDone?.Invoke(obj);

    public static void InvokeOnShowGameplayScreen() => ShowGameplayScreen?.Invoke();

    public static void InvokeOnAllButtonsPressed() => AllButtonsPressed?.Invoke();

    public static void InvokeOnDoneButtonPressed() => DoneButtonPressed?.Invoke();

    public static void InvokeOnCloseLiftDoors() => CloseLiftDoors?.Invoke();
}
