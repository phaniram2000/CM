using UnityEngine;

namespace RPS
{

	public class NpcAnimationHelper : MonoBehaviour
	{
		public void NpcSlappingAnimation()
		{
			RPSGameEvents.InvokeOnNpcGaveSlap();
		}
	}

}
