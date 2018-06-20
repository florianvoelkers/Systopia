using UnityEngine;

public class QuestActivationReaction : Reaction {

	public Quest quest;
	public PlayerQuests playerQuests;

	protected override void ImmediateReaction (){
		playerQuests.AddQuest (quest);
	}
}
