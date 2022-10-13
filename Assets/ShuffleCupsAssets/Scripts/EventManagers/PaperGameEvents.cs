using System;
using UnityEngine;

namespace ShuffleCups
{
	public class PaperGameEvents : MonoBehaviour
	{
		public static PaperGameEvents Singleton;

		public Action tapToPlay;
	
		public Action pullPaperStep;
		public Action<float> paperPullDelta;

		public Action tearPaper, playerCrossFinishLine, aiCrossFinishLine;
	
		private void Awake()
		{
			if (!Singleton) Singleton = this;
			else Destroy(gameObject);
		}

		public void InvokeTapToPlay() => tapToPlay?.Invoke();
	
		public void InvokePullPaperStep() => pullPaperStep?.Invoke();

		public void InvokePullPaperDelta(float delta) => paperPullDelta?.Invoke(delta);
	
		public void InvokeTearPaper() => tearPaper?.Invoke();

		public void InvokeAiCrossFinishLine() => aiCrossFinishLine?.Invoke();
	
		public void InvokeCrossFinishLine() => playerCrossFinishLine?.Invoke();
	}
}



