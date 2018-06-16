using UnityEngine;

[System.Serializable]
public class DialogueOption {

	[TextArea (2,3)] public string text;
	public int destinationNodeId;
	public ReactionCollection reactionCollection;

	public DialogueOption (string text, int destinationNodeId) {
		this.text = text;
		this.destinationNodeId = destinationNodeId;
	}
}
