using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using StateMachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameMode
{
	Classroom, Toilet, Pub, Bank, PhoneUnlockPattern, Cheat,TitForTat, Kiss, HotChips,YesOrNo,Banana,Blackmail,Board, PriceTag, GoldDigger,CarChase,CarTheft,Bail,StealItems, Jackpot, StealAndRun,FlexRun,ShiftRun
}
public class GameRules : MonoBehaviour
{
	public bool hasPreDrawCam;
	public bool hasSeenPreDrawCam;
	[Header("Will be set automatically, if you have a IRuleSet component here"),
	 SerializeField] private GameMode currentGameMode;

	[SerializeField] private List<Component> ruleSets;

	public static IRuleSet GetRuleSet => Get._currentRuleSets.Count > 0 ? Get._currentRuleSets[^1] : null;
	public static GameMode GetGameMode => Get.currentGameMode;

	public static GameRules Get { get; set; }

	private List<IRuleSet> _currentRuleSets = new();

	private void OnEnable()
	{
		SetGameMode();

		SceneManager.sceneUnloaded += SceneManagerOnSceneUnloaded;
		GameEvents.DoneWithRuleSet += OnDoneWithRuleSet;
	}

	private void OnDisable()
	{
		SceneManager.sceneUnloaded -= SceneManagerOnSceneUnloaded;
		GameEvents.DoneWithRuleSet -= OnDoneWithRuleSet;
	}

	private void Awake()
	{
		if (!Get) Get = this;
		else Destroy(gameObject);
		
		if(!GetComponent<AInputHandler>()) print("No InputHandler added.");
		
		Vibration.Init();
	}

	private void OnValidate()
	{
		if(Application.isPlaying) return;

		foreach (var rule in ruleSets)
		{
			if (rule is not IRuleSet)
			{
				ruleSets.Remove(rule);
				continue;
			}

			_currentRuleSets.Add(rule as IRuleSet);
		}
	}

	private void OnDoneWithRuleSet()
	{
		if(_currentRuleSets.Count > 0)
			_currentRuleSets.RemoveAt(_currentRuleSets.Count - 1);
	}
	public void SetGameMode()
	{
		if(ruleSets.Count == 0)
		{
			_currentRuleSets = GetComponents<IRuleSet>().ToList();
			foreach (var ruleSet in _currentRuleSets) ruleSets.Add((Component)ruleSet);
			print("No IRuleSet component added to ruleSets List. Added " + ruleSets.Count + " sets from GameObject.");
		}
		else
		{
			foreach (var ruleSet in ruleSets) _currentRuleSets.Add((IRuleSet)ruleSet);
		}

		if(_currentRuleSets.Count > 0)
			currentGameMode = _currentRuleSets[^1] switch
			{
				ToiletRuleSet => GameMode.Toilet,
				ClassroomRuleSet => GameMode.Classroom,
				PubRuleSet => GameMode.Pub,
				BankRuleSet => GameMode.Bank,
				PatternRuleSet => GameMode.PhoneUnlockPattern,
				CheatingRuleSet => GameMode.Cheat,
				KissRuleSet => GameMode.Kiss,
				_ => currentGameMode
			};
	}
	
	private static void SceneManagerOnSceneUnloaded(Scene scene) => DOTween.KillAll();
}