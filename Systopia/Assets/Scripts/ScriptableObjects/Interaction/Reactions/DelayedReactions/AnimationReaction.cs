using UnityEngine;

public class AnimationReaction : DelayedReaction {

	public Animator animator; // animation that will have its trigger parameter set
	public string trigger; // the name of the trigger parameter to be set

	private int triggerHash; // hash representing the trigger parameter to be set

	protected override void SpecificInit () {
		triggerHash = Animator.StringToHash (trigger);
	}

	protected override void ImmediaReaction () {
		animator.SetTrigger (triggerHash);
	}
}
