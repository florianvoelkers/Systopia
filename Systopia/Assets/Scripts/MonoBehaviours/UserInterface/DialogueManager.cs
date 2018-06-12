using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

	[SerializeField] private GameObject dialogueBack;
	[SerializeField] private Text npcText;
	[SerializeField] private GameObject optionsObject;
	[SerializeField] private GameObject optionsPrefab;
	[SerializeField] private Text npcName;
	[SerializeField] private Text dialogueSentence;
	[SerializeField] private GameObject continueButton;

	private List <DialogueNode> nodes;
	private int currentNodeId;
	private int currentSentence;
	private List <GameObject> dialogueOptions = new List<GameObject> ();

	private void OnEnable () {
		dialogueBack.SetActive (false);
		npcText.text = "";
		npcName.text = "";
		dialogueSentence.text = "";
		continueButton.SetActive(false);
	}

	public void DisplayDialog (List <DialogueNode> dialogueNodes, string npcName) {
		nodes = dialogueNodes;
		currentNodeId = 0;
		currentSentence = -1;
		dialogueBack.SetActive (true);
		this.npcName.enabled = true;
		this.npcName.text = npcName;
		continueButton.SetActive (true);
		DisplayNextSentence ();
	}

	public void DisplayNextSentence () {
		currentSentence++;
		if (currentSentence < nodes [currentNodeId].sentences.Length) {
			dialogueSentence.text = nodes [currentNodeId].sentences [currentSentence];
		} else {
			if (nodes [currentNodeId].options.Count > 0) {
				DisplayOptions ();
			} else {
				EndDialogue ();
			}
		}
	}

	private void DisplayOptions () {
		npcName.enabled = false;
		dialogueSentence.text = "";
		npcText.text = nodes [currentNodeId].sentences [currentSentence - 1];
		continueButton.SetActive (false);
		for (int i = 0; i < nodes[currentNodeId].options.Count; i++) {
			dialogueOptions.Add (Instantiate (optionsPrefab, optionsObject.transform));
			dialogueOptions [i].GetComponent <Text> ().text = nodes [currentNodeId].options [i].text;
			int localIndex = i;
			UnityEngine.Events.UnityAction optionSelection = () => {
				this.SelectOption (localIndex);
			};
			dialogueOptions [i].GetComponent <Button> ().onClick.AddListener (optionSelection);
		}
	}

	private void SelectOption (int selectedOption) {
		for (int i = optionsObject.transform.childCount - 1; i >= 0; i--) {
			Destroy (optionsObject.transform.GetChild (i).gameObject);
		}
		dialogueOptions.Clear ();
		npcText.text = "";
		currentSentence = -1;
		npcName.enabled = true;
		currentNodeId = nodes [currentNodeId].options [selectedOption].destinationNodeId;
		continueButton.SetActive (true);
		DisplayNextSentence ();
	}

	private void EndDialogue () {
		nodes = null;
		currentNodeId = 0;
		currentSentence = -1;
		dialogueBack.SetActive (false);
		dialogueSentence.text = "";
		npcName.enabled = false;
		npcName.text = "";
		continueButton.SetActive (false);
	}
}
