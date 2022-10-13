using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Eflatun.SceneReference;
using UnityEngine.SceneManagement;

public class StoryMissionInfo : MonoBehaviour
{
	[System.Serializable] public class MissionInfo
	{
		public string title, description;
		public int reward = 100;
		public Sprite image;
		public SceneReference destScene;

		public AudioClip voiceClip;
		
	}

	[SerializeField] private List<MissionInfo> missions;
	
	[SerializeField] private TextMeshPro missionNum, title, description, reward;
	[SerializeField] private SpriteRenderer photo;

	private MissionInfo _currentMission;
	private Camera _cam;
	private AudioSource _audio;
	private bool _isDone;

	private void Awake() => DOTween.KillAll();

	private void Start()
	{
		_audio = GetComponent<AudioSource>();
		_cam = Camera.main;
		var levelNo = PlayerPrefs.GetInt("levelNo", 1);
		missionNum.text = "Mission " + levelNo;

		var buildIndex = PlayerPrefs.GetInt("lastBuildIndex", 2);
		_currentMission = missions.Find(mission => mission.destScene.BuildIndex == buildIndex);

		if (_currentMission == null)
		{
			print("no mission found for " + buildIndex);
			return;
		}

		title.text = _currentMission.title;
		description.text = _currentMission.description;
		reward.text = "$" + _currentMission.reward;
		photo.sprite = _currentMission.image;
	    if(_currentMission.title is not null) GAScript.MissionName = _currentMission.title;
	}

	private void Update()
	{
		if(_isDone) return;
		if(Input.GetKeyDown(KeyCode.N)) { NextLevel(); return; }
		
		var ray = _cam.ScreenPointToRay(InputExtensions.GetInputPosition());
		if(!Physics.Raycast(ray, out var hit, 10f)) return;
		if(!hit.collider.CompareTag("Player")) return;

		hit.transform.DOScale(hit.transform.localScale * 0.85f, 0.0175f)
			.SetLoops(2, LoopType.Yoyo)
			.OnComplete(NextLevel);
		NextLevel();
		Vibration.Vibrate(30);
	}

	private void NextLevel()
	{
		_isDone = true;
		_audio.Play();
		
		PlayerPrefs.SetInt("CurrentLevelEarnings", _currentMission.reward);
		
		SceneManager.LoadScene(_currentMission.destScene.BuildIndex);
	}
}