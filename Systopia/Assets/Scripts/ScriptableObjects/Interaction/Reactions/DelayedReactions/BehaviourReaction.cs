using UnityEngine;

// turns on/off behaviours, e.g. MonoBehaviours, Animators, AudioSources etc.
public class BehaviourReaction : DelayedReaction {

	public Behaviour behaviour;
	public bool enabledState;

	protected override void ImmediateReaction () {
		behaviour.enabled = enabledState;
	}
}
