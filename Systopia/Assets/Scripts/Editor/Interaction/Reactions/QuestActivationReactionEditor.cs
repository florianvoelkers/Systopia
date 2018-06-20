using UnityEditor;

[CustomEditor (typeof (QuestActivationReaction))]
public class QuestActivationReactionEditor : ReactionEditor {

	protected override string GetFoldoutLabel () {
		return "Quest Activation Reaction";
	}
}
