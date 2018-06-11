using UnityEngine;

[System.Serializable]
public class DialogueOption {

	[TextArea (2,5)] public string text;
	public int destinationNodeId;

	public DialogueOption (string text, int destinationNodeId) {
		this.text = text;
		this.destinationNodeId = destinationNodeId;
	}
}
