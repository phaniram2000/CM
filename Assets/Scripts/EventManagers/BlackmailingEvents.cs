using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BlackmailingEvents
{
	public static event Action FoundTakingPictures;
	
	public static event Action StartTakingPictures;
	
	public static event Action StopTakingPictures;

	public static event Action TakePicture;

	public static event Action ToKiss;

	public static event Action ToObserve;

	public static event Action ToNextGamePhase;

	public static event Action GotFooled;
	public static event Action FinalWin;
	public static event Action FinalLose;

	public static event Action SayNo;
	public static event Action ToInterruptTheSequence;

	public static void InvokeFoundTakingPictures() => FoundTakingPictures?.Invoke();
	
	public static void InvokeStartTakingPictures() => StartTakingPictures?.Invoke();
	
	public static void InvokeStopTakingPictures() => StopTakingPictures?.Invoke();
	
	public static void InvokeTakePicture() => TakePicture?.Invoke();
	
	public static void InvokeToKiss() => ToKiss?.Invoke();
	
	public static void InvokeToObserve() => ToObserve?.Invoke();
	
	public static void InvokeToNextGamePhase() => ToNextGamePhase?.Invoke();
	
	public static void InvokeGotFooled() => GotFooled?.Invoke();
	public static void InvokeFinalWin() => FinalWin?.Invoke();
	public static void InvokeFinalLose() => FinalLose?.Invoke();
	public static void InvokeSayNo() => SayNo?.Invoke();
	public static void InvokeToInterruptTheSequence() => ToInterruptTheSequence?.Invoke();
}
