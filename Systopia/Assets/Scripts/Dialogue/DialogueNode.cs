using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueNode {

	public int nodeId = -1;
	[TextArea (2,3)] public string [] sentences;
	public List <DialogueOption> options;
}
