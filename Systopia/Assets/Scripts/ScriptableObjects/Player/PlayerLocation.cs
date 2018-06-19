using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu (menuName = "Player/Location")]
public class PlayerLocation : ScriptableObject {

	public string startingPositionName;
	public bool currentPositionSet;
	public Vector3 currentPosition;
	public Quaternion currentRotation;
	public string currentSceneName;

	public void Reset () {
		startingPositionName = "";
		currentPositionSet = false;
		currentPosition = new Vector3 (0f, 0f, 0f);
		currentSceneName = "";
	}
}
