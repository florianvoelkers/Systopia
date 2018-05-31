using UnityEditor;

[CustomEditor (typeof (PickedUpItemReaction))]
public class PickedUpItemReactionEditor : ReactionEditor {

	protected override string GetFoldoutLabel () {
		return "Pick Up Item Reaction";
	}
}
