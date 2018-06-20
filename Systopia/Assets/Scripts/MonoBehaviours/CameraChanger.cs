using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChanger : MonoBehaviour {

	[SerializeField] private GameObject firstCamera;
	[SerializeField] private GameObject secondCamera;

	void OnTriggerExit (Collider other) {
		if (other.tag == "Player") 
			SwitchCamera ();
	}

	private void SwitchCamera () {
		firstCamera.SetActive (!firstCamera.activeSelf);
		secondCamera.SetActive (!secondCamera.activeSelf);
	}
}
