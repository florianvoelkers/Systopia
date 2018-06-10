using UnityEngine;

[System.Serializable]
public class Stat : ScriptableObject {

	public int baseValue;
	public int statBonus;
	public int finalValue;
	public string statName; 
	public string statDescription; 

	public void AddBonus (int bonus) {
		statBonus += bonus;
	}

	public void RemoveBonus (int bonus) {
		statBonus -= bonus;
	}

	public void SetBaseValue (int value) {
		baseValue = value;
	}

	public int GetValue () {
		finalValue = baseValue + statBonus;
		return finalValue;
	}
}
