using UnityEngine;

public class QuestReaction : Reaction {

	public Quest quest;
	public State state;

	protected override void ImmediateReaction (){
		quest.FinishState (state);
	}
}
