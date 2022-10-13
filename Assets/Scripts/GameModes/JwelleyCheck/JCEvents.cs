using System;
using UnityEngine;


public static partial class JCEvents
{
    public static event Action<GameObject> JwelleryItemSelected,JwelleryReadyToCrush;

    public static event Action AllowToCheckJwellery;

    public static event Action JwelleryMeterCheckDone;

    public static event Action FakeSelected, RealSelected;

    public static event Action JwelleryCrushingDone;


    public static event Action<bool,bool> CheckJwelleryPlace;
    public static event Action SelectNextJwelleryItem;

    public static event Action CloseLocker,MakeGirlRunWithLocker,MakeLockerTransformToGirlHand;

    public static event Action MakeGirlSad;


}

public static partial class JCEvents
{
    

    public static void InvokeOnAllowToCheckJwellery() => AllowToCheckJwellery?.Invoke();

    public static void InvokeOnJwelleryItemSelected(GameObject obj) => JwelleryItemSelected?.Invoke(obj);

    public static void InvokeOnJwelleryMeterCheckDone() => JwelleryMeterCheckDone?.Invoke();

    public static void InvokeOnFakeSelected() => FakeSelected?.Invoke();

    public static void InvokeOnRealSelected() => RealSelected?.Invoke();

    public static void InvokeOnJwelleryReadyToCrush(GameObject obj) => JwelleryReadyToCrush?.Invoke(obj);

    public static void InvokeOnJwelleryCrushingDone() => JwelleryCrushingDone?.Invoke();

    public static void InvokeOnSelectNextJwelleryItem() => SelectNextJwelleryItem?.Invoke();

    public static void InvokeOnCheckJwelleryPlace(bool arg1, bool arg2) => CheckJwelleryPlace?.Invoke(arg1, arg2);

    public static void InvokeOnCloseLocker() => CloseLocker?.Invoke();

    public static void InvokeOnMakeGirlRunWithLocker() => MakeGirlRunWithLocker?.Invoke();

    public static void InvokeOnMakeLockerTransformToGirlHand() => MakeLockerTransformToGirlHand?.Invoke();

    public static void InvokeOnMakeGirlSad() => MakeGirlSad?.Invoke();
}
