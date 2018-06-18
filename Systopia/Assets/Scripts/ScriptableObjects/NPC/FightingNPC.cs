﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu (menuName = "NPC/Fighter")]
public class FightingNPC : ScriptableObject {

	public string npcName;
	public Sprite npcIcon;
	public int currentHP;
	public int maximumHP;
	public int damage;
	public int accuracy;
	public int armor;

	public int Attack () {
		int randomNumber = Random.Range (1, 100);
		if (randomNumber <= accuracy) {
			return damage;
		}
		return 0;
	}

	public bool TakeDamage (int amount) {
		amount -= armor;
		currentHP -= amount;
		if (currentHP <= 0) {
			return true;
		}
		return false;
	}
}
