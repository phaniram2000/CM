using System.Collections;
using UnityEngine;


namespace ShuffleCups
{
	public class PaperPullerAntiHeroAI : MonoBehaviour
    {
    	//needs optimisation for Random.range
    	[SerializeField] private float[] pullDeltas, pullDeltaTimes, waitTimes;
    
    	public float increaseMultiplier, decreaseMultiplier;
    	private PaperPullerPlayer _player;
    	private PaperPullerData _data;
    
    	private Coroutine _logic;
    	private bool _isInPlay = true;
    
    	private void OnEnable()
    	{
    		PaperGameEvents.Singleton.tapToPlay += OnGameStart;
    		
    		PaperGameEvents.Singleton.tearPaper += OnGameOver;
    		PaperGameEvents.Singleton.playerCrossFinishLine += OnGameOver;

            global::GameEvents.TapToPlay += OnGameStart;
        }
    	
    	private void OnDisable()
    	{
    		PaperGameEvents.Singleton.tapToPlay -= OnGameStart;
    		
    		PaperGameEvents.Singleton.tearPaper -= OnGameOver;
    		PaperGameEvents.Singleton.playerCrossFinishLine -= OnGameOver;
            
            global::GameEvents.TapToPlay -= OnGameStart;
    	}
    
    	private void Start()
    	{
    		_data = GetComponent<PaperPullerPlayer>().myData;
    		_player = GetComponent<PaperPullerPlayer>();
    	}
    
    	private void Update()
    	{
    		if (_data.currentRpm > 0.1f)
    			_data.currentRpm -= Time.deltaTime * decreaseMultiplier;
    	}
    
    	private IEnumerator LightsCameraAction()
    	{
    		while (_isInPlay)
    		{
    			var timeTaken = pullDeltaTimes[Random.Range(0, pullDeltaTimes.Length)];
    			
    			while (timeTaken > 0f)
    			{
    				timeTaken -= Time.deltaTime;
    				
    				var delta = pullDeltas[Random.Range(0, pullDeltas.Length)];
    				var old = _data.currentRpm;
    
    				_data.currentRpm += Time.deltaTime * increaseMultiplier * delta;
    				_data.distanceFromZero -= (_data.currentRpm - old) * _data.pullingSpeed;
    				_player.EnemyPullStep(delta * Time.deltaTime);
    				yield return null;
    			}
    			
    			var currentPullingStateTime = waitTimes[Random.Range(0, waitTimes.Length)];
    			_player.EndEnemyPull();
    			
    			yield return GameExtensions.GetWaiter(currentPullingStateTime);
    		}
    	}
    
    	private void OnGameStart()
    	{
    		_logic = StartCoroutine(LightsCameraAction());
    	}
    	
    	private void OnGameOver()
    	{
    		_isInPlay = false;
    		StopCoroutine(_logic);
    	}
    }
}

