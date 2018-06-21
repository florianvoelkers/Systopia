using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintingRiddleController : MonoBehaviour {

	[SerializeField] private GameObject paintingRiddleCanvas;
	[SerializeField] private InputField inputField;
	[SerializeField] private string solution;
	[SerializeField] private ReactionCollection solutionCorrectReaction;

	public void CheckSolution () {
		if (inputField.text == solution) {
			paintingRiddleCanvas.SetActive (false);
			solutionCorrectReaction.React ();
		}
	}
}
