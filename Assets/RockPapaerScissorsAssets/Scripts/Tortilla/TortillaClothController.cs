using UnityEngine;


namespace RPS
{

	public class TortillaClothController : MonoBehaviour
	{
		private Cloth _cloth;

		private void OnEnable()
		{
			RPSGameEvents.OnTapToPlay += OnTapToPlay;
			RPSGameEvents.NewRound += OnNewRound;
			RPSGameEvents.CameraZoom += OnCameraZoom;
			RPSGameEvents.NpcGaveSlap += OnNpcGaveSlap;
		}

		private void OnDisable()
		{
			RPSGameEvents.OnTapToPlay -= OnTapToPlay;
			RPSGameEvents.NewRound -= OnNewRound;
			RPSGameEvents.CameraZoom -= OnCameraZoom;
			RPSGameEvents.NpcGaveSlap -= OnNpcGaveSlap;
		}

		private void Start()
		{
			_cloth = GetComponent<Cloth>();
		}

		private void OnTapToPlay()
		{
			_cloth.useGravity = false;
		}

		private void OnNewRound()
		{
			_cloth.useGravity = true;
		}

		private void OnCameraZoom()
		{
			_cloth.useGravity = false;
		}

		private void OnNpcGaveSlap()
		{
			_cloth.useGravity = true;
		}

	}

}
