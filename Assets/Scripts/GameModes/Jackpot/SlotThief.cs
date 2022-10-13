using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SlotThief : MonoBehaviour
{
	[SerializeField] private float rotationSpeed;
	[SerializeField] private GameObject detectionRayObject;
	[SerializeField] private LayerMask interactiveLayers;

	[SerializeField] private List<GameObject> slots;
	[SerializeField] private Transform lever;
	[SerializeField] private Transform leverDirectionTransform;
	[SerializeField] private Transform sidePositionTransform;
	
	private Animator _anim;
	private static readonly int ToPullHash = Animator.StringToHash("ToPull");
	private static readonly int VictoryHash = Animator.StringToHash("Victory");
	private static readonly int DefeatHash = Animator.StringToHash("Defeat");
	
	private Camera _camera;

	private int _totalSlots = 3;
	

	private void OnEnable()
	{
		GameEvents.TapToPlay += OnTapToPlay;
		GameEvents.PressDoneButton += CheckForTheCombination;
	}

	private void OnDisable()
	{
		GameEvents.TapToPlay -= OnTapToPlay;
		GameEvents.PressDoneButton += CheckForTheCombination;
	}

	private void Start()
	{
		_anim = GetComponent<Animator>();
		_camera = Camera.main;
		RotateLever();
	}
	public void Rotate(float x)
	{
		var ray = _camera.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out var hit, 100f,interactiveLayers))
		{
			hit.transform.Rotate(Vector3.up,x * rotationSpeed * Time.deltaTime);
		}

		// transform.Rotate(Vector3.forward,x * rotationSpeed * Time.deltaTime);
	}
	
	public void CheckForCode()
	{
		var ray = new Ray(detectionRayObject.transform.position, Vector3.left);
		if (Physics.Raycast(ray, out var hit, 3f))
		{
			if (slots.Count <= 0) return;
			
			if (slots[0] == hit.collider.gameObject)
			{
				_totalSlots--;
				slots[0].transform.parent.GetComponent<MeshCollider>().enabled = false;
				slots[0].transform.parent = null;
				slots[0].SetActive(false);
				slots.RemoveAt(0);
				if (_totalSlots == 0)
				{
					//play the slot lever pull animations
					PullTheLever();
				}
			}
		}
		Debug.DrawRay(ray.origin, ray.direction * 3 , Color.black, 3f);
	}

	private void RotateLever()
	{
		lever.transform.DOLocalRotate(lever.transform.rotation.eulerAngles - Vector3.right * 30f, 2f).SetLoops(2,LoopType.Yoyo);
	}

	private void PullTheLever()
	{
		transform.DOLookAt(leverDirectionTransform.position, 0.1f).SetEase(Ease.Linear);
		transform.DOMove(leverDirectionTransform.position, 0.25f).SetEase(Ease.Linear);
		
		_anim.SetTrigger(ToPullHash);
		if (_totalSlots != 0)
		{
			DOVirtual.DelayedCall(1f, ()=> _anim.SetTrigger(DefeatHash));
			
		}
		else
			DOVirtual.DelayedCall(1f, ()=> _anim.SetTrigger(VictoryHash));
	}

	private void OnTapToPlay()
	{
		//Rotate Player to side
		transform.DOMove(sidePositionTransform.position,1f).SetEase(Ease.Linear);
//		transform.DOLookAt(leverDirectionTransform.position, 0.2f).SetEase(Ease.Linear);
	}

	private void CheckForTheCombination()
	{
		PullTheLever();
	}
	
}
