using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Ritchie : MonoBehaviour
{
	[System.Serializable] public struct VoiceLines
	{
		public string text;
		public AudioClip audio;
	}
	
    [SerializeField] private Animator file;
	[SerializeField] private Transform movepoint, acceptButton;
	[SerializeField] private Transform cat, catmoviepoint;
	
	[SerializeField] private TextMeshPro dialogue;
	[SerializeField] private List<VoiceLines> voiceLines;

	[SerializeField] private AudioClip meaow, fileopen;
	private AudioSource audioSource;
	private Tween _acceptTween;
	private static readonly int Story = Animator.StringToHash("Story");

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
		_acceptTween = acceptButton.DOScale(0f, 0.5f).From();
		_acceptTween.Pause();
		
		dialogue.transform.parent.DOScale(0f, 0.5f).From().SetEase(Ease.OutBack);
		var voiceLineIndex = PlayerPrefs.GetInt("voiceLineIndex", 0);
		
		var voiceLine = voiceLines[voiceLineIndex % voiceLines.Count];
		dialogue.text = voiceLine.text;
		audioSource.PlayOneShot(voiceLine.audio,1f);
		
		PlayerPrefs.SetInt("voiceLineIndex", ++voiceLineIndex);
		
		DOVirtual.DelayedCall(2, () => GetComponent<Animator>().SetTrigger(Story));
		DOVirtual.DelayedCall(2, () => audioSource.PlayOneShot(meaow,.5f));
	}
    
    public void TakingFile()
	{
		var initScale = dialogue.transform.parent.localScale; 
		dialogue.transform.parent.DOScale(0f, 0.25f);
		dialogue.transform.parent.DOScale(initScale, 0.25f).SetEase(Ease.OutBack);
		dialogue.text = "Here's your NEXT MISSION";
		
        file.transform.parent = null;
        file.transform.DOMove(movepoint.transform.position, .2f);
        file.transform.DORotate(new Vector3(75, 0, 360), .2f);
        file.transform.DOScale(movepoint.transform.localScale, .2f).OnComplete(() =>
        {
	        
            file.enabled = true;
			audioSource.PlayOneShot(fileopen, .3f);
            cat.DOMoveY(catmoviepoint.position.y, 1f);
            audioSource.PlayOneShot(meaow,.5f);
			DOVirtual.DelayedCall(1f, () => _acceptTween.Play());
        });
    }
}