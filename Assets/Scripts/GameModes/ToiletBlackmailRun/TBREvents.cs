using System;
using UnityEngine;


public static partial class TBREvents
{
    public static event Action GirlStaringToOpenDoor, GirlOpenDoorDone, GirlCloseDoorDone,GirlStartingToLockDoor,GirlDoorLockingDone;

    public static event Action BoyAngryDone;

    public static event Action OnTapToLock;

    public static event Action<int> ItemsButtonPressed;

    public static event Action<GameObject> ItemReadyToPick;

    public static event Action ItemPickedUpByGirl;

    public static event Action GirlPrankingDoneNowEscape;

}

public static partial class TBREvents 
{
    public static void InvokeOnGirlStaringToOpenDoor() => GirlStaringToOpenDoor?.Invoke();


    public static void InvokeOnGirlOpenDoorDone() => GirlOpenDoorDone?.Invoke();

    public static void InvokeOnBoyAngryDone() => BoyAngryDone?.Invoke();

    public static void InvokeOnGirlCloseDoorDone() => GirlCloseDoorDone?.Invoke();

    public static void InvokeOnTapToLock() => OnTapToLock?.Invoke();

    public static void InvokeOnGirlStartingToLockDoor() => GirlStartingToLockDoor?.Invoke();


    public static void InvokeOnGirlDoorLockingDone() => GirlDoorLockingDone?.Invoke();

    public static void InvokeOnItemsButtonPressed(int obj) => ItemsButtonPressed?.Invoke(obj);

    public static void InvokeOnItemReadyToPick(GameObject obj) => ItemReadyToPick?.Invoke(obj);

    public static void InvokeOnItemPickedUpByGirl() => ItemPickedUpByGirl?.Invoke();

    public static void InvokeOnGirlPrankingDoneNowEscape() => GirlPrankingDoneNowEscape?.Invoke();
}
