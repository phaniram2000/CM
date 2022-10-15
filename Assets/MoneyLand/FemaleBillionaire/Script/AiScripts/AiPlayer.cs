using UnityEngine;
using UnityEngine.AI;

public class AiPlayer : MonoBehaviour
{
	public NavMeshAgent navMeshAgent;
	public bool canMove;
    public int num;
    public float thresholdDistance;
    public Transform handTransform;
	private AiParent aiparent;

	private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
		aiparent = AiParent.instance;
    }

	private void Update()
    {
        navMeshAgent.speed = GameManager.instance.aiSpeed;
        thresholdDistance = aiparent.thresholdDistance;
        if (canMove)
        {
            CheckDestinationReached();
            if (num < aiparent.aiMovingTransforms.Length)
            {
                if (navMeshAgent.destination != aiparent.aiMovingTransforms[num].position)
					navMeshAgent.SetDestination(aiparent.aiMovingTransforms[num].position);
			}
            else
            {
                navMeshAgent.ResetPath();
            }
        }
    }
    private void CheckDestinationReached()
    {
        if (num < aiparent.aiMovingTransforms.Length)
        {
            float distanceToTarget = Vector3.Distance(transform.position, aiparent.aiMovingTransforms[num].position);
            if (distanceToTarget < thresholdDistance)
            {
                num = num + 1;
            }
        }
        if (num > 2)
        {
            print("Add To Line");
        }
    }
}
