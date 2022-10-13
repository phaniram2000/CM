using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;


namespace ShuffleCups
{
	public class CupController : MonoBehaviour
{
	private static readonly List<Sequence> CupSelectionSequences = new List<Sequence>();
	
	[SerializeField] private Transform ballHolder, handTarget;
	[SerializeField] private float selectionPosZ = -0.5f;
	
	[SerializeField] private Transform emoji;
	[SerializeField] private Material emojiMat;

	[SerializeField] private MeshRenderer mugBody;
	[SerializeField] private Material selectionMat;
	[SerializeField] private Color rightColor, wrongColor;

	private Rigidbody _rb;
	private Transform _ball;

	private bool _shouldExit;

	private void OnEnable()
	{
		GameEvents.Singleton.ShuffleEnd += OnShuffleEnd;

		GameEvents.Singleton.MakeSelection += OnMakeSelection;
	}
	
	private void OnDisable()
	{
		GameEvents.Singleton.ShuffleEnd -= OnShuffleEnd;

		GameEvents.Singleton.MakeSelection -= OnMakeSelection;
	}

	private void Start()
	{
		_rb = GetComponent<Rigidbody>();
		emojiMat = emoji.GetComponent<Renderer>().sharedMaterial;
	}

	public void AcceptBall(Transform ball)
	{
		var position = transform.position;

		transform.DORotate(Vector3.left * 90f, .5f, RotateMode.LocalAxisAdd).SetLoops(2, LoopType.Yoyo);
		transform.DOMove(new Vector3(position.x, position.y + 1.5f, position.z), .5f).SetLoops(2, LoopType.Yoyo);
	
		ball.DOMove(ballHolder.position, 1f).OnComplete(() =>
		{
			ball.parent = ballHolder;
		});
	}

	private bool HasBall()
	{
		return ballHolder.childCount > 0;
	}

	private void EjectBall()
	{
		_ball = ballHolder.GetChild(0);
		_ball.parent = null;

		CupSelectionSequences.Add(GetSequence());
	}

	private Sequence GetSequence()
	{
		var seq = DOTween.Sequence();
		seq.Append(_ball.DOMoveY(transform.position.y + 3.5f, 0.5f));
		seq.AppendCallback(() => AudioManager.instance.Play("hasBall"));
		_ball.rotation = Quaternion.identity;
		
		if (!LevelFlowController.only.isHatLevel) return seq;
		//if (!LevelFlowController.only.isHatLevel) yield break;
		
		seq.OnComplete(() => 
			_ball.DORotate(Vector3.right * 60f, 1f).OnComplete(() => 
				_ball.DORotate(Vector3.up * 720f, 6f, RotateMode.WorldAxisAdd).SetLoops(-1, LoopType.Incremental)));

		return seq;
	}

	public Transform GetHandTarget(Vector3 leftHand, Vector3 rightHand, out bool isLeftHand, out bool exit)
	{
		isLeftHand = Vector3.Distance(handTarget.position, leftHand) < Vector3.Distance(handTarget.position, rightHand);
		exit = _shouldExit;
		_shouldExit = true;

		return handTarget;
	}

	private void ShowEmoji()
	{
		emoji.gameObject.SetActive(true);

		emojiMat.mainTexture = LevelFlowController.only.emojis[Random.Range(0, LevelFlowController.only.emojis.Length)];
		
		emoji.parent = null;
		emoji.eulerAngles = new Vector3(180f, 0, 180f);
		
		var sequence = DOTween.Sequence();

		sequence.Append(emoji.DOScale(emoji.localScale * 2f, 0.5f));
		sequence.Insert(0f, emoji.DOMoveY(4f, 0.5f));
		sequence.Append(emoji.DOShakeRotation(1.5f, Vector3.forward * 23f, 5, 45f));
		sequence.InsertCallback(1f, () => AudioManager.instance.Play("hasntBall"));
		//sequence.InsertCallback(1.5f, () => AudioManager.instance.Play("fail"));
		sequence.Append(emoji.DOMoveY(15f, .5f).SetEase(Ease.InElastic));
	}

	private void OnShuffleEnd()
	{
		StartCoroutine(ConcludeShuffle());
	}

	private IEnumerator ConcludeShuffle()
	{
		//4 is a magic number because till the time shuffles are happening 2 cups will have 2 tweens, one in x and z each
		yield return new WaitUntil(() => DOTween.TotalActiveTweens() < 4);

		_rb.isKinematic = true;
		transform.DOMoveZ(selectionPosZ, 1f).OnComplete(()=>GameEvents.Singleton.InvokeShowInstruction());
	}

	private void OnMakeSelection(bool isLeftHand, CupController cup)
	{
		if (cup != this) return;
		
		_rb.isKinematic = false;
		var result = HasBall();
		var myColor = result ? rightColor : wrongColor;

		if (result)
			EjectBall();
		else
			ShowEmoji();
		mugBody.sharedMaterial = selectionMat;
		DOTween.To(() => mugBody.material.color, value => mugBody.material.color = value, myColor, 1f);

		transform.DORotate(Vector3.forward * (180f * (isLeftHand ? 1f : -1f)), .5f, RotateMode.WorldAxisAdd);
		
		transform.DOMoveY(2.5f, 1f).OnComplete(() => LevelFlowController.only.ProcessCupSelectionResult(result));
	}
}
}

