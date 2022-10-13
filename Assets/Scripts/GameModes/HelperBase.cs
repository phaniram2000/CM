using System;
using UnityEngine;

public class HelperBase : MonoBehaviour
{
	public virtual Action OnSwapComplete() => null;

	public virtual Func<int, string> ResultTextFormatter() => null;
}