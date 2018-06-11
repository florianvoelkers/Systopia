using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueNode {

	public int nodeId = -1;
	[TextArea (3,5)] public string text;
	public List <DialogueOption> options;

	public DialogueNode (string text) {
		this.text = text;
		options = new List<DialogueOption> ();
	}
}
