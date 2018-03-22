using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SelectAllOfTag : ScriptableWizard {

	public string searchTag =  "Your tag here";

	[MenuItem ("My Tools/Select all of tag...")]
	static void SelectAllOfTagWizard () {
		ScriptableWizard.DisplayWizard<SelectAllOfTag> ("Select all of tag...", "Make selection");
	}

	void OnWizardCreate () {
		GameObject[] gameObjects = GameObject.FindGameObjectsWithTag (searchTag);
		Selection.objects = gameObjects;
	}
}
