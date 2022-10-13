using UnityEngine;


namespace RPS
{

	public class PlayerHelmetController : MonoBehaviour
	{
		[SerializeField] private int helmetMaxHit;
		[SerializeField] private GameObject helmetGameObject;

		private Rigidbody _rb;


		private CharacterRefBank _refBank;

		private bool _isPlayerHelmetOn, _isNpcHelmetOn;

		public bool IsPlayerHelmetOn => _isPlayerHelmetOn;

		public bool IsNpcHelmetOn => _isNpcHelmetOn;

		private int _hitCounter;
		[SerializeField] private float regularForce;
		[SerializeField] private float upForce;

		private void OnEnable()
		{
			RPSGameEvents.PlayerHelmetEnable += OnPlayerHelmetEnable;
			RPSGameEvents.NpcGaveSlap += OnNpcGaveSlap;

		}

		private void OnDisable()
		{
			RPSGameEvents.PlayerHelmetEnable -= OnPlayerHelmetEnable;
			RPSGameEvents.NpcGaveSlap -= OnNpcGaveSlap;
		}

		private void Start()
		{
			_refBank = transform.root.GetComponent<CharacterRefBank>();
			_hitCounter = 0;
			helmetGameObject = transform.GetChild(0).gameObject;
			helmetGameObject.SetActive(false);
			_rb = GetComponent<Rigidbody>();
		}

		private void OnPlayerHelmetEnable()
		{
			_isPlayerHelmetOn = true;
			helmetGameObject.SetActive(true);
		}

		private void OnNpcGaveSlap()
		{
			if (!_isPlayerHelmetOn) return;

			_hitCounter++;

			if (_hitCounter <= helmetMaxHit) return;

			_isPlayerHelmetOn = false;
			//to tell helmet ka hogaya.
			transform.parent = null;
			_refBank.Controller.OnPlayerHelmetFall();
			_rb.isKinematic = false;
			var direction = -transform.right;
			_rb.AddForce(direction * (regularForce) + Vector3.up * upForce, ForceMode.Impulse);

		}
	}


}
