using System.Collections.Generic;

[System.Serializable]
public class DialogueNode {

	public int nodeId = -1;
	public string text;
	public List <DialogueOption> options;

	public DialogueNode (string text) {
		this.text = text;
		options = new List<DialogueOption> ();
	}
}
