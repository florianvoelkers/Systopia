using UnityEditor;

[CustomEditor (typeof (FightReaction))]
public class FightReactionEditor : ReactionEditor {

	protected override string GetFoldoutLabel () {
		return "Fight Reaction";
	}
}
