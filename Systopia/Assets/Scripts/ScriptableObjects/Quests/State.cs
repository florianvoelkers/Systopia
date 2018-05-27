using UnityEngine;

[System.Serializable]
[CreateAssetMenu (menuName = "State")]
public class State : ScriptableObject {

	public string stateName;
	public string stateDescription;
	public bool isStateFinished;
}
