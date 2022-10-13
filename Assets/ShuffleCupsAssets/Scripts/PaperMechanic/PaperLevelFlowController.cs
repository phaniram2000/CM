using DG.Tweening;
using UnityEngine;

namespace ShuffleCups
{
	public class PaperLevelFlowController : MonoBehaviour
    {
    	public static PaperLevelFlowController only;
    
    	public Transform suitcaseWinPos;
    	[Range(0, 1f)] public float warningPercentageRpm, deadPercentageRpm;
    	
    	[SerializeField] public GameObject[] winParticles;
    
    	private bool _subscribedToStep;
    
    	private PaperPullerPlayer _player;
    	
    	public PaperPullerPlayer Player => _player;
    	
    	public float decreaseMultiplier = 1f, increaseMultiplier = 1f;
    
    	private void OnEnable()
    	{
    		PaperGameEvents.Singleton.playerCrossFinishLine += OnPlayerWin;
    		
    		PaperGameEvents.Singleton.tearPaper += OnTearPaper;
    		_subscribedToStep = true;
    	}
    
    	private void OnDisable()
    	{
    		PaperGameEvents.Singleton.playerCrossFinishLine -= OnPlayerWin;
    		
    		if(!_subscribedToStep) return;
    		
    		PaperGameEvents.Singleton.tearPaper -= OnTearPaper;
    		_subscribedToStep = false;
    	}
    
    	private void Awake()
    	{
    		if (!only) only = this;
    		else Destroy(gameObject);
    		
    		_player = GameObject.FindGameObjectWithTag("Player").GetComponent<PaperPullerPlayer>();
    		DOTween.KillAll();
    	}
    
    	private void OnTearPaper()
    	{
    		Invoke(nameof(OnDisable), 0.2f);
    	}
    	
    	private void OnPlayerWin()
    	{
    		DOTween.Sequence().AppendInterval(5f).AppendCallback(() =>
    		{
    			AudioManager.instance.Play("confetti");
    			AudioManager.instance.Play("fatake");
    			foreach (var particle in winParticles)
    				particle.SetActive(true);
    		});
    	}
    }
}

