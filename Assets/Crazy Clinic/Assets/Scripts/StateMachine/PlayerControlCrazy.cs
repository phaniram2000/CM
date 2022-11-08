using UnityEngine;

public class PlayerControlCrazy : MonoBehaviour
{

	private Animator _animator;
	private Camera _camera;

	private bool _hasTappedToPlay;

	private void OnEnable()
	{
		//GameEvents.TapToPlay += OnTapToPlay;
	}

	private void OnDisable()
	{
		//GameEvents.TapToPlay -= OnTapToPlay;
	}

	private void Awake()
	{
	}

	private void Start()
	{
		_animator = GetComponent<Animator>();
		_camera = Camera.main;
	}
	
	private void Update()
	{
		if (InputHandlerCrazy.GetFingerDown())
		{
			if (!_hasTappedToPlay)
			{
				GameEventsCrazy.InvokeTapToPlay();
				_hasTappedToPlay = true;
			}
		}
	}


	private bool HasTappedToPlay()
	{
		if (_hasTappedToPlay) return true;
		
		//if(InputHandler.GetFingerDown())
			//GameEvents.InvokeTapToPlay();
		
		_hasTappedToPlay = true;
		return _hasTappedToPlay;
	}

	private void CutTheCone()
	{
		
	}
}
