using DG.Tweening;
using UnityEngine;


namespace ShuffleCups
{
	public class PaperCupController : MonoBehaviour
	{
		[SerializeField] private GameObject waterLevel;
		[SerializeField] private GameObject puddle;

		private bool _hasCreatedPuddle;
	
		private void OnCollisionEnter(Collision other)
		{
			if(_hasCreatedPuddle) return;
			if(!other.collider.CompareTag("Table")) return;

			waterLevel.SetActive(false);
			for (int i = 0; i < other.contactCount; i++)
			{
				if(_hasCreatedPuddle) return;
				var currPoint = other.contacts[i].point;
				if(Vector3.Distance(transform.position + transform.up , currPoint) > Vector3.Distance(transform.position - transform.up, currPoint))
					continue;
			
				//if point is at bottom of cup, don't spawn puddle there
				var spawnPoint = new Vector3(transform.position.x, currPoint.y - 0.075f, currPoint.z);
				var puddlax = Instantiate(puddle, spawnPoint, puddle.transform.rotation).transform;

				puddlax.localScale = Vector3.one * 0.75f; 
				puddlax.DOMoveY(currPoint.y + 0.1f, 2f);
				puddlax.DOScale(Vector3.one * 1.25f, 2f);

				DOTween.Sequence().AppendInterval(2f).AppendCallback(() => transform.GetChild(0).gameObject.SetActive(false));
				return;
			}
		}
	}
}

