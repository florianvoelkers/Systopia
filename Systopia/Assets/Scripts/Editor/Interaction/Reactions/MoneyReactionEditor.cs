using UnityEditor;

[CustomEditor (typeof (MoneyReaction))]
public class MoneyReactionEditor : ReactionEditor {

	protected override string GetFoldoutLabel () {
		return "Money Reaction";
	}
}