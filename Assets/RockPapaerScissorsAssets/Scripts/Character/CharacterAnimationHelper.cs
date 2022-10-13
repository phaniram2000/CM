using UnityEngine;


namespace RPS
{

	public class CharacterAnimationHelper : MonoBehaviour
	{
		public void PlayerSlappingAnimation()
		{
			RPSGameEvents.InvokeOnPlayerGaveSlap();
		}
	}
}