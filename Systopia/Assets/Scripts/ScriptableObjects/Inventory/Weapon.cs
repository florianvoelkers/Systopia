using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item {

	public List <StatBonus> bonusses = new List<StatBonus> ();

	public override void Use () {
		Debug.Log ("equip weapon");
	}
}
