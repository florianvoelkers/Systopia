using UnityEngine;

[System.Serializable]
public class StatBonus : ScriptableObject {

	public int bonus;
	public Stat stat;

	public StatBonus (int bonus, Stat stat) {
		this.bonus = bonus;
		this.stat = stat;
	}
}
