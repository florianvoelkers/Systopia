using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueReaction : Reaction {

	public string nPCName = "";
	public List <DialogueNode> dialogueNodes = new List<DialogueNode> ();

	private DialogueManager dialogueManager;

	protected override void SpecificInit () {
		dialogueManager = FindObjectOfType<DialogueManager> ();
	}

	public void AddNode (DialogueNode newNode) {
		dialogueNodes.Add (newNode);
	}

	public void RemoveNode (DialogueNode nodeToRemove) {
		dialogueNodes.Remove (nodeToRemove);
	}

	protected override void ImmediateReaction () {
		dialogueManager.DisplayDialog (dialogueNodes, nPCName);
	}
}
