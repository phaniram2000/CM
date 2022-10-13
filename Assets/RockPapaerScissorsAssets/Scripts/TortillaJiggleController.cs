using FIMSpace.Jiggling;
using UnityEngine;

namespace RPS
{

	public class TortillaJiggleController : MonoBehaviour
	{
		private FJiggling_Simple _jiggleSimple;

		private void Start()
		{
			_jiggleSimple = GetComponent<FJiggling_Simple>();
			_jiggleSimple.StartJiggle(1);
		}
	}

}
