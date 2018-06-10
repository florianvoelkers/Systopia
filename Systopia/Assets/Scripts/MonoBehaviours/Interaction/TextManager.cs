using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TextManager : MonoBehaviour {

	public struct Instruction {
		public string message;
		public Color textColor;
		public float startTime;
	}

	public Text text;
	public GameObject dialogBackground;
	public float displayTimePerCharacter = 0.1f;
	public float additionalDisplayTime = 0.5f;
	public GameObject optionsText;
	public GameObject dialogOptions;
	public Text lastNPCText;

	private List <GameObject> optionsTexts = new List<GameObject> ();
	private List <Instruction> instructions = new List <Instruction> ();
	private float clearTime;

	private void Update () {
		if (instructions.Count > 0 && Time.time >= instructions [0].startTime) {
			dialogBackground.SetActive (true);
			text.text = instructions [0].message;
			text.color = instructions [0].textColor;
			instructions.RemoveAt (0);
		} else if (Time.time >= clearTime) {
			text.text = string.Empty;
			dialogBackground.SetActive (false);
		}
	}

	public void DisplayMessageWithOptions (string message, Color textColor, float delay, string [] options) {
		StartCoroutine (DisplayMessageWithOptionsWithDelay (message, textColor, delay, options));
	}

	public void DisplayMessage (string message, Color textColor, float delay) {
		float startTime = Time.time + delay;
		float displayDuration = message.Length * displayTimePerCharacter + additionalDisplayTime;
		float newClearTime = startTime + displayDuration;

		if (newClearTime > clearTime)
			clearTime = newClearTime;

		Instruction newInstruction = new Instruction {
			message = message,
			textColor = textColor,
			startTime = startTime
		};
		instructions.Add (newInstruction);
		SortInstructions ();
	}

	private void SortInstructions () {
		for (int i = 0; i < instructions.Count; i++) {
			bool swapped = false;

			for (int j = 0; j < instructions.Count; j++) {
				if (instructions [i].startTime > instructions [j].startTime) {
					Instruction temp = instructions [i];
					instructions [i] = instructions [j];
					instructions [j] = temp;

					swapped = true;
				}
			}

			if (!swapped)
				break;
		}
	}

	private IEnumerator DisplayMessageWithOptionsWithDelay(string message, Color textColor, float delay, string [] options) {
		yield return new WaitForSeconds (delay);
		dialogBackground.SetActive (true);
		lastNPCText.gameObject.SetActive (true);
		dialogOptions.SetActive (true);
		lastNPCText.text = message;
		for (int i = 0; i < options.Length; i++) {
			optionsTexts.Add (Instantiate (optionsText, dialogOptions.transform));
			optionsTexts [optionsTexts.Count - 1].transform.GetComponent <Text> ().text = options [i];
			optionsTexts [optionsTexts.Count - 1].transform.GetComponent <Text> ().color = textColor;
		}

	}
}
