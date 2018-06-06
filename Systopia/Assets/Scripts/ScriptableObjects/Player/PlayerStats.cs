using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu (menuName = "Player/Stats")]
public class PlayerStats : ScriptableObject {

	public Stat [] stats;
}
