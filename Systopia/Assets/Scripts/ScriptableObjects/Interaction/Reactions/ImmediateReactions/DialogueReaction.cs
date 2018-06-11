using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueReaction : Reaction {

	public List <DialogueNode> dialogueNodes = new List<DialogueNode> ();

	public void AddNode (DialogueNode newNode) {
		dialogueNodes.Add (newNode);
	}

	public void RemoveNode (DialogueNode nodeToRemove) {
		dialogueNodes.Remove (nodeToRemove);
	}

	protected override void ImmediateReaction () {

	}
}
