using UnityEngine;

namespace RPS
{

	public class NpcRefBank : MonoBehaviour
	{
		public bool isdead;

		public CharacterRefBank CharacterRefBank { get; private set; }

		public NpcAnimatorController AnimationController { get; private set; }

		public NpcController Controller { get; private set; }

		public NpcRagdollController RagdollController { get; private set; }

		private void Start()
		{
			AnimationController = GetComponent<NpcAnimatorController>();
			Controller = GetComponent<NpcController>();
			CharacterRefBank = GameObject.FindWithTag("PlayerCharacter").GetComponent<CharacterRefBank>();
			RagdollController = GetComponent<NpcRagdollController>();
		}
	}

}
