using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Animations.Rigging;


namespace ShuffleCups
{
	public class PaperPullerPlayer : MonoBehaviour
{
	public PaperPullerData myData;
	public float GetMaxRpm => myData.maxRpm; 
	public float GetCurrentRpm => myData.currentRpm;
	
	[SerializeField] private Rig spineRig, leftHandRig, rightHandRig;
	[SerializeField] private Transform roll, paper;
	[SerializeField] private Transform leftHandTarget, rightHandTarget;

	private Material _paperMaterial;
	private Vector2 _textureScale, _textureOffset;
	
	private Animator _anim;
	private Quaternion _initLeftHandRot, _initRightHandRot;
	private TweenerCore<Quaternion, Quaternion, NoOptions> _leftTween, _rightTween;
	
	private static readonly int Defeat = Animator.StringToHash("defeat");
	private static readonly int Die = Animator.StringToHash("die");
	private static readonly int IsEnemyFloat = Animator.StringToHash("isEnemyFloat");
	private static readonly int IsEnemyBool = Animator.StringToHash("isEnemyBool");
	private static readonly int Win = Animator.StringToHash("Win");
	
	private static readonly int BumpMap = Shader.PropertyToID("_BumpMap");
	private static readonly int BaseMap = Shader.PropertyToID("_BaseMap");

	private void OnEnable()
	{
		PaperGameEvents.Singleton.pullPaperStep += OnPullPaperStep;
		PaperGameEvents.Singleton.paperPullDelta += OnPaperPullDelta;
		
		PaperGameEvents.Singleton.tearPaper += OnPaperTear;
		PaperGameEvents.Singleton.aiCrossFinishLine += OnAiCrossFinishLine;
		PaperGameEvents.Singleton.playerCrossFinishLine += OnPlayerCrossFinishLine;
	}

	private void OnDisable()
	{
		PaperGameEvents.Singleton.pullPaperStep -= OnPullPaperStep;
		PaperGameEvents.Singleton.paperPullDelta -= OnPaperPullDelta;
		
		PaperGameEvents.Singleton.tearPaper -= OnPaperTear;
		PaperGameEvents.Singleton.aiCrossFinishLine -= OnAiCrossFinishLine;
		PaperGameEvents.Singleton.playerCrossFinishLine -= OnPlayerCrossFinishLine;
	}
	
	private void Start()
	{
		_anim = GetComponent<Animator>();
		myData.source = GetComponent<AudioSource>(); 
		
		_paperMaterial = paper.GetComponent<MeshRenderer>().material;
		
		_anim.SetFloat(IsEnemyFloat, myData.isPlayer ? 1f : -1f, 0.5f, 1);
		_anim.SetBool(IsEnemyBool, !myData.isPlayer);
		
		myData.zPosMax = paper.position.z;
		myData.zScaleMax = paper.localScale.z;
		
		myData.yPosMin = roll.position.y;
		myData.yScaleMin = roll.localScale.y;

		_initLeftHandRot = leftHandTarget.rotation;
		_initRightHandRot = rightHandTarget.rotation;
	}
	
	private bool HasLostGame => myData.currentRpm > (myData.maxRpm * PaperLevelFlowController.only.deadPercentageRpm);

	private void UpdateTransforms()
	{
		if(!myData.isPaperTorn)
			myData.cupHoldingPaper.position = myData.cupHoldingPaperDest.position;

		UpdatePaperTransform();

		var scale = Mathf.Lerp(myData.yScaleMin, myData.yScaleMax, 1 - myData.distanceFromZero);
		roll.localScale = new Vector3(roll.localScale.x, scale, scale);
		roll.position = new Vector3(roll.position.x, Mathf.Lerp(myData.yPosMin, myData.yPosMax, 1 - myData.distanceFromZero), roll.position.z);

		_textureScale = Vector2.right + Vector2.up * (myData.distanceFromZero * 10f);
		_textureOffset = Vector2.right + Vector2.up * ((1 - myData.distanceFromZero) * 10f);
		
		_paperMaterial.SetTextureScale(BaseMap, _textureScale);
		_paperMaterial.SetTextureScale(BumpMap, _textureScale);
		
		_paperMaterial.SetTextureOffset(BaseMap, _textureOffset);
		_paperMaterial.SetTextureOffset(BumpMap, _textureOffset);
	}

	private void UpdatePaperTransform()
	{
		paper.localScale = new Vector3(paper.localScale.x, paper.localScale.y,
			Mathf.Lerp(myData.zScaleMin, myData.zScaleMax, myData.distanceFromZero));
		paper.position = new Vector3(paper.position.x, paper.position.y,
			Mathf.Lerp(myData.zPosMin, myData.zPosMax, myData.distanceFromZero));
	}

	private void RotatePaperRoll(float delta)
	{
		var rotation = Vector3.left * (delta * .5f);
		
		roll.Rotate(rotation, Space.World);
		
		leftHandTarget.Rotate(rotation, Space.World);
		rightHandTarget.Rotate(rotation, Space.World);
	}
	
	
	public void RevertBackToOriginalHandRotations()
	{
		if(_leftTween != null)
			if (_leftTween.IsActive() || _rightTween.IsActive())
			{
				_leftTween.Kill();
				_rightTween.Kill();
			}
			
		_leftTween = leftHandTarget.DORotateQuaternion(_initLeftHandRot, 0.25f);
		_rightTween = rightHandTarget.DORotateQuaternion(_initRightHandRot, 0.25f);
	}
	
	public void EnemyPullStep(float delta)
	{
		OnPaperPullDelta(delta);
		UpdateTransforms();
	}

	public void EndEnemyPull()
	{
		RevertBackToOriginalHandRotations();
	}

	private void PaperTear()
	{
		_anim.SetTrigger(Defeat);
		DOTween.To(() => _anim.GetLayerWeight(1), value => _anim.SetLayerWeight(1, value), 1f, .5f);

		if(spineRig)
			DOTween.To(() => spineRig.weight, value => spineRig.weight = value, 0f, 1f);
		//DOTween.To(() => leftHandRig.weight, value => leftHandRig.weight = value, 0f, .7f);
		leftHandRig.weight = 0f;
		DOTween.To(() => rightHandRig.weight, value => rightHandRig.weight = value, 0f, 1f);

		myData.isPaperTorn = true;
		myData.distanceFromZero -= 0.05f;

		myData.waterCup.transform.parent = null;
		myData.waterCup.isKinematic = false;
		myData.waterCup.AddTorque(Vector3.left * 90f, ForceMode.Impulse);
		myData.waterCup.AddForce(Vector3.forward * 2.5f, ForceMode.Impulse);

		UpdatePaperTransform();
	}

	private void OnPullPaperStep()
	{
		if(HasLostGame)
		{
			PaperTear();
			PaperGameEvents.Singleton.InvokeTearPaper();
			
			return;
		}
		UpdateTransforms();
	}
	
	private void OnPaperPullDelta(float delta)
	{
		RotatePaperRoll(delta);
	}

	private void OnPaperTear()
	{
		if(myData.isPlayer) return;

		if (myData.isGirl)
			DOTween.Sequence().AppendInterval(1f).AppendCallback(() => _anim.SetTrigger(Win));
		else
			_anim.SetTrigger(Win);
		
		GetComponentInChildren<Collider>().isTrigger = true;
		paper.parent.parent = null;

		if(spineRig)
			DOTween.To(() => spineRig.weight, value => spineRig.weight = value, myData.isGirl ? 0f : 0.5f, 1f);
		DOTween.To(() => leftHandRig.weight, value => leftHandRig.weight = value, myData.isGirl ? 0f : 0.5f, 1f);
		DOTween.To(() => rightHandRig.weight, value => rightHandRig.weight = value, myData.isGirl ? 0f : 0.5f, 1f);
	}

	private void OnAiCrossFinishLine()
	{
		if (myData.isPlayer)
		{
			_anim.SetTrigger(Defeat);
		}
		else
		{
			_anim.SetTrigger(Win);
			GetComponentInChildren<Collider>().isTrigger = true;
			paper.parent.parent = null;
			
		}
		
		if(spineRig)
			DOTween.To(() => spineRig.weight, value => spineRig.weight = value, 0f, 1f);
		DOTween.To(() => leftHandRig.weight, value => leftHandRig.weight = value, 0f, 1f);
		DOTween.To(() => rightHandRig.weight, value => rightHandRig.weight = value, 0f, 1f);
	}
	
	private void OnPlayerCrossFinishLine()
	{
		if (myData.isPlayer)
		{
			_anim.SetTrigger(Win);
			GetComponentInChildren<Collider>().isTrigger = true;
			paper.parent.parent = null;
		}
		else
			_anim.SetTrigger(Defeat);
		
		if(spineRig)
			DOTween.To(() => spineRig.weight, value => spineRig.weight = value, 0f, 1f);
		DOTween.To(() => leftHandRig.weight, value => leftHandRig.weight = value, 0f, 1f);
		DOTween.To(() => rightHandRig.weight, value => rightHandRig.weight = value, 0f, 1f);
	}

	private void OnCollisionEnter(Collision other)
	{
		if(!other.collider.CompareTag("Missile")) return;
		
		Vibration.Vibrate(30);
		var seq = DOTween.Sequence();
		seq.AppendCallback(() => _anim.SetTrigger(Die));
		seq.AppendInterval(0.25f);
		seq.AppendCallback(() =>
		{
			AudioManager.instance.Play("thud");
			if(myData.cake)
				myData.cake.Play();
			
			if(spineRig)
            	DOTween.To(() => spineRig.weight, value => spineRig.weight = value, 0f, 1f);
            DOTween.To(() => leftHandRig.weight, value => leftHandRig.weight = value, 0f, 1f);
			DOTween.To(() => rightHandRig.weight, value => rightHandRig.weight = value, 0f, 1f);
			
			DOTween.To(() => _anim.GetLayerWeight(1), value => _anim.SetLayerWeight(1, value), 0f, 1f);
		});
		var rb = other.gameObject.GetComponent<Rigidbody>();
		rb.velocity = rb.velocity.normalized;
	}
}

[System.Serializable] public class PaperPullerData
{
	public bool isPlayer, isGirl;
	
	[Header("Pulling")] public bool isPaperTorn;
	[Range(0f, 0.05f)] public float pullingSpeed = 0.015f;
	public Transform cupHoldingPaper, cupHoldingPaperDest;
	public Rigidbody waterCup;
	public ParticleSystem speed, cake;
	[HideInInspector] public AudioSource source;
	
	[Header("Paper Scaling")] public float zScaleMin;
	public float zPosMin;
	[HideInInspector] public float zPosMax, zScaleMax;
	
	[Header("Paper Roll Scaling")] public float yPosMax;
	public float yScaleMax;
	[HideInInspector] public float yScaleMin, yPosMin;

	[Header("Lerping")] public float distanceFromZero = 1f;
	public float maxRpm = 50f, currentRpm;
}
}

