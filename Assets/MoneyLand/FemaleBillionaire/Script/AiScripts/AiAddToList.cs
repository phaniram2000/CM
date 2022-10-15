using UnityEngine;
using UnityEngine.AI;

public class AiAddToList : MonoBehaviour
{
	private AiParent _aiP;
	private static readonly int Walk = Animator.StringToHash("Walk");

	private void Start()
	{
		_aiP = AiParent.instance;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 6) return;
		if(!other.TryGetComponent(out Animator anim)) return;
		
		anim.SetBool(Walk, false);
		if (!AiParent.instance.spawnedAiList.Contains(other.gameObject))
			AiParent.instance.spawnedAiList.Add(other.gameObject);
		other.GetComponent<AiPlayer>().num = 0;
		other.GetComponent<NavMeshAgent>().ResetPath();
		other.GetComponent<AiPlayer>().canMove = false;
		var t = other.GetComponent<AiPlayer>().handTransform;
		for (var i = 0; i < t.childCount; i++) 
			t.GetChild(i).gameObject.SetActive(false);

		var number = AiParent.instance.spawnedAiList.Count - 1;
		_aiP.spawnedAiList[number].transform.position = _aiP.spawnPos[number].position;
		_aiP.spawnedAiList[number].transform.rotation = _aiP.spawnPos[number].rotation;
	}
}