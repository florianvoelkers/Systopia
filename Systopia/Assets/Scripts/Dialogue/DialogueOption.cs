[System.Serializable]
public class DialogueOption {

	public string text;
	public int destinationNodeId;

	public DialogueOption (string text, int destinationNodeId) {
		this.text = text;
		this.destinationNodeId = destinationNodeId;
	}
}
