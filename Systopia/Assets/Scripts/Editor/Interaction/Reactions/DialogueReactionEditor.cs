using UnityEditor;

[CustomEditor (typeof (DialogueReaction))]
public class DialogueReactionEditor : ReactionEditor {

	private SerializedProperty dialogueNodesProperty;
	private const string dialogueReactionPropNodesProperty = "dialogueNodes";

	protected override void Init () {
		dialogueNodesProperty = serializedObject.FindProperty (dialogueReactionPropNodesProperty);
	}

	protected override string GetFoldoutLabel () {
		return "Dialogue Reaction";
	}
}