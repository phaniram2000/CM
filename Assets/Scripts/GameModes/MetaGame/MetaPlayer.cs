using UnityEngine;

public class MetaPlayer : MonoBehaviour
{
	[SerializeField] private float movementSpeed;

	private Rigidbody _rb;
	private Transform _transform;
	private Animator _anim;
	private static readonly int MoveBlend = Animator.StringToHash("moveBlend");

	private void Start()
	{
		_rb = GetComponent<Rigidbody>();
		_anim = GetComponent<Animator>();
		_transform = transform;
	}

	public void UpdatePlayer(in float moveSpeed, in Quaternion desiredRot)
	{
		_anim.SetFloat(MoveBlend, moveSpeed);
		_rb.MoveRotation(desiredRot);
		_rb.MovePosition(_transform.position + _transform.forward * (moveSpeed * movementSpeed * Time.fixedDeltaTime));
	}

	public void StopMoving()
	{
		_anim.SetFloat(MoveBlend, 0f);
	}
}