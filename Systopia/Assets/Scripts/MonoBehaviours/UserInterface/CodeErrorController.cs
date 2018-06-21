using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodeErrorController : MonoBehaviour {

	[SerializeField] private GameObject codeErrorCanvas;

	public void HideCanvas () {
		codeErrorCanvas.SetActive (false);
	}
}
