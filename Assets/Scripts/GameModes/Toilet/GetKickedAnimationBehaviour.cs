using UnityEngine;

public class GetKickedAnimationBehaviour : StateMachineBehaviour
{
	private enum StateType { EntryState, ExitState}
	[SerializeField] private StateType thisStateType;
	private ToiletSplineFollower _npc;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (thisStateType != StateType.EntryState) return;
		
		if (!_npc) _npc = animator.GetComponent<ToiletSplineFollower>();

		_npc.StopFollowing();
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (thisStateType != StateType.ExitState) return;
		
		if (!_npc) _npc = animator.GetComponent<ToiletSplineFollower>();
			
		_npc.MoveAfterKick();
	}

	// OnStateIK is called right after Animator.OnAnimatorIK()
	// public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	// {
	// 	// Implement code that sets up animation IK (inverse kinematics)
	// }

	// OnStateMove is called right after Animator.OnAnimatorMove()
	// public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	// {
	// 	// Implement code that processes and affects root motion
	// }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	// public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	// {
	// }
}