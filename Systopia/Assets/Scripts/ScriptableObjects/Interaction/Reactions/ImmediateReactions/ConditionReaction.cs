using UnityEngine;

// references one condition in the AllCondition asset
public class ConditionReaction : Reaction {

	public Condition condition;
	public bool satisfied;

	protected override void ImmediateReaction () {
		condition.satisfied = satisfied;
	}
}
