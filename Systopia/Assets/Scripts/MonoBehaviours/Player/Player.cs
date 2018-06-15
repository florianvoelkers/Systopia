using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	[Header ("Items")]
	[SerializeField] private PlayerEquipment playerEquipment;
	[Header ("Stats")]
	[SerializeField] private PlayerStats stats;
	[SerializeField] private Stat strength;
	[SerializeField] private Stat dexterity;
	[SerializeField] private Stat vitality;
	[SerializeField] private Stat damage;
	[SerializeField] private Stat armor;
	[SerializeField] private Stat accuracy;
	[SerializeField] private Stat health;
	[Header ("Health")]
	[SerializeField] private IntVariable hp;

	private void Start () {
		stats.ResetBonus ();
		CalculateStatsFromEquippedItems ();
		CalculateCombinedStats ();
	}

	private void CalculateStatsFromEquippedItems () {
		playerEquipment.CalculateStats ();
	}

	private void CalculateCombinedStats () {
		int damageBaseValue = strength.GetValue () + Mathf.RoundToInt(dexterity.GetValue () * 0.5f);
		damage.SetBaseValue (damageBaseValue);
		int accuracyBaseValue = dexterity.GetValue () * 5;
		accuracy.SetBaseValue (accuracyBaseValue);
		int healthBaseValue = vitality.GetValue () * 10;
		health.SetBaseValue (healthBaseValue);
		hp.SetValue (health.GetValue ());
		hp.maxValue = health.GetValue ();
	}
}
