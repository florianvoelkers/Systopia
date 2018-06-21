using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnfangsSzenenScript : MonoBehaviour {

    private TextManager textmanager;
	private SceneController sceneController;

	// Use this for initialization
	void Start () {
        textmanager = FindObjectOfType<TextManager>();
		sceneController = FindObjectOfType<SceneController> ();
	}
	//textmanager.DisplayMessage(message,textColor,delay);
	//sceneController.StartGame ();
	// Update is called once per frame
	void Update () {
		
	}
}
