using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPS
{

	public class CharacterRefBank : MonoBehaviour
	{
		public bool isdead;

		public NpcController NpcController { get; private set; }
		public CharacterAnimationController AnimationController { get; private set; }

		public CharacterController Controller { get; private set; }

		public CharacterRagdollController RagdollController { get; private set; }

		private void Start()
		{
			AnimationController = GetComponent<CharacterAnimationController>();
			Controller = GetComponent<CharacterController>();
			NpcController = GameObject.FindWithTag("NpcCharacter").GetComponent<NpcController>();
			RagdollController = GetComponent<CharacterRagdollController>();
		}
	}

}
