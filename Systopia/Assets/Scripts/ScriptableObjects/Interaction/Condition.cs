using UnityEngine;

[System.Serializable]
[CreateAssetMenu (menuName = "Interaction/Condition")]
public class Condition : ScriptableObject {

	public string description;
	public bool satisfied;
	public int hash;
}
