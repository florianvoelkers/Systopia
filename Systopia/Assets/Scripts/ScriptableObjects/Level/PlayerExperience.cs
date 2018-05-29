using UnityEngine;

[System.Serializable]
[CreateAssetMenu (menuName = "Player/Experience")]
public class PlayerExperience : ScriptableObject {

	public int experience;

	public void AddExperience (int gainedExperience) {
		experience += gainedExperience;
	}

	public void LoseExperience (int lostExperience) {
		experience -= lostExperience;
	}

	public void SetExperience (int currentExperience) {
		experience = currentExperience;
	}
}
