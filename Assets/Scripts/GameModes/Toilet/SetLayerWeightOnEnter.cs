using UnityEngine;

public class SetLayerWeightOnEnter : StateMachineBehaviour
{
	[SerializeField] private float tweenDuration, layerWeight;
	private static readonly int AnimationSpeed = Animator.StringToHash("animationSpeed");

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		StartWaving(animator);
		animator.SetFloat(AnimationSpeed, Random.Range(0.5f, 1f));
	}
	
	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		StopWaving(animator);
	}

	// OnStateMove is called right after Animator.OnAnimatorMove()
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    // Implement code that processes and affects root motion
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK()
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    // Implement code that sets up animation IK (inverse kinematics)
	//}

	private void StopWaving(Animator animator) => TweenLayerWeightTo(animator, 0f);

	private void StartWaving(Animator animator) => TweenLayerWeightTo(animator, layerWeight);

	private void TweenLayerWeightTo(Animator anim, in float value) => MyHelpers.TweenAnimatorLayerWeightTo(anim, 1, value, tweenDuration);
}