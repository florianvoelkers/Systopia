using UnityEditor;

[CustomEditor (typeof (LocationReaction))]
public class LocationReactionEditor : ReactionEditor {

	protected override string GetFoldoutLabel () {
		return "Location Reaction";
	}
}
