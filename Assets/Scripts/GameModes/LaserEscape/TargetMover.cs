using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class TargetMover : MonoBehaviour
{
	[SerializeField] private Transform bone;
	[SerializeField] private float minimumDistanceThreshold = 0.1f;
	[SerializeField] private SpriteRenderer sprite;
	[SerializeField] private Transform resetTransform;

	[SerializeField] private Rig rig; 
	
	public bool isSelected;
	private BoxCollider _collider;
	private bool _toDetect;

	private void OnEnable()
	{
		LaserEscapeEvents.HitWithLaser += OnEscapingAllColliders;
		LaserEscapeEvents.EscapedAllTheLasers += OnEscapingAllColliders;
		LaserEscapeEvents.ResetTargetPositions += OnResetTargetPositions;
		LaserEscapeEvents.EnableRigs += EnableRig;
	}

	private void OnDisable()
	{
		LaserEscapeEvents.HitWithLaser -= OnEscapingAllColliders;
		LaserEscapeEvents.EscapedAllTheLasers -= OnEscapingAllColliders;
		LaserEscapeEvents.ResetTargetPositions -= OnResetTargetPositions;
		LaserEscapeEvents.EnableRigs -= EnableRig;
	}

	private void Start()
	{
		_collider = GetComponent<BoxCollider>();
		DisableRig();
		DisableSprite();
	}
	
	private void LateUpdate()
	{
		if (!_toDetect) return;
		
		if (!isSelected) return;
		
		var dist = Vector3.Distance(transform.position, bone.position);
		if (dist >= minimumDistanceThreshold)
		{
			transform.position = bone.position;
		}
		DetectLaser();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Laser")) return;
		
		LaserEscapeEvents.InvokeHitWithLaser();
		DisableSprite();
		if (AudioManager.instance)
		{
			AudioManager.instance.Play("Shock");
			AudioManager.instance.Play("Scream");
		}
		Vibration.Vibrate(30);
	}

	private void DetectLaser()
	{
		//only work when sprite is active
		var ray = new Ray(transform.position, Vector3.down);

		if (Physics.Raycast(ray, out var hit, 50f))
		{
			sprite.color = hit.collider.CompareTag("Laser") ? Color.red : Color.green;
			var spriteColor = sprite.color;
			spriteColor.a = 0.5f;
			sprite.color = spriteColor;
			Debug.DrawRay(ray.origin,ray.direction * 50f, Color.black,3f);
			print(hit.collider.gameObject.name);
		}
	}

	private void OnEscapingAllColliders()
	{
		_collider.enabled = false;
		DisableRig();
	}

	private void OnResetTargetPositions()
	{
		transform.DOMove(resetTransform.position, 0.05f);
		DisableSprite();
		_toDetect = false;
	}

	private void DisableRig()
	{
		rig.weight = 0f;
	}
	private void EnableRig()
	{
		rig.weight = 1f;
		EnableSprite();
		_toDetect = true;
		DetectLaser();
	}

	private void EnableSprite()
	{
		sprite.gameObject.SetActive(true);
	}

	private void DisableSprite()
	{
		sprite.gameObject.SetActive(false);
	}
}
