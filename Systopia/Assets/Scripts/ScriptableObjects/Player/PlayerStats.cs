using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu (menuName = "Player/Stats")]
public class PlayerStats : ScriptableObject {

	public string playerName;
	public Stat [] stats;

	public void ResetBonus () {
		for (int i = 0; i < stats.Length; i++) {
			stats [i].statBonus = 0;
		}
	}
}
