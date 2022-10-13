using DG.Tweening;
using UnityEngine;


namespace RPS
{

	public class CharacterRagdollController : MonoBehaviour
	{
		[SerializeField] private Rigidbody[] rigidbodies;
		[SerializeField] private float regularForce, upForce;

		[Header("Change color on death"), SerializeField]
		private Renderer skin;

		private CharacterRefBank _my;

		private Material _material;
		[SerializeField] private bool shouldTurnToGrey;
		[SerializeField] private int toChangeMatIndex;
		[SerializeField] private Color deadColor;



		private void Start()
		{
			_my = GetComponent<CharacterRefBank>();
			foreach (var rb in rigidbodies) rb.isKinematic = true;
			if (shouldTurnToGrey)
				_material = skin.materials[toChangeMatIndex];
		}

		public void GoRagdoll()
		{
			_my.AnimationController.SetAnimatorStatus(false);

			var direction = -transform.forward;
			foreach (var rb in rigidbodies)
			{
				rb.isKinematic = false;
				rb.AddForce(direction * (regularForce) + Vector3.up * upForce, ForceMode.Impulse);
			}

			if (shouldTurnToGrey)
				_material.DOColor(deadColor, 1f);

		}

		public void UnKinematicise()
		{
			foreach (var rb in rigidbodies) rb.isKinematic = false;
		}
	}
}
