using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item {

	public List <StatBonus> bonusses = new List<StatBonus> ();
	public PlayerEquipment playerEquipment;

	public override void Use () {
		playerEquipment.EquipWeapon (this);
	}
}
