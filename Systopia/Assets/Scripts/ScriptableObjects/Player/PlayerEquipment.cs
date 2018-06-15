using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu (menuName = "Player/Equipment")]
public class PlayerEquipment : ScriptableObject {

	public Wearable headItem;
	public Wearable bodyItem;
	public Wearable legsItem;
	public Wearable fingerItem;
	public Weapon weapon;
	public IntVariable hp;

	private void UnequipAll () {
		UnequipHead ();
		UnequipBody ();
		UnequipLegs ();
		UnequipFinger ();
		UnequipWeapon ();
	}

	private void OnDisable () {
		
	}

	public void CalculateStats () {
		if (headItem) {
			CalculateStatFromEquippedWearable (headItem);
			headItem.isEquipped = true;
		}
		if (bodyItem) {
			CalculateStatFromEquippedWearable (bodyItem);
			bodyItem.isEquipped = true;
		}
		if (legsItem) {
			CalculateStatFromEquippedWearable (legsItem);
			legsItem.isEquipped = true;
		}
		if (fingerItem) {
			CalculateStatFromEquippedWearable (fingerItem);
			fingerItem.isEquipped = true;
		}
		if (weapon) {
			CalculateStatFromEquippedWeapon (weapon);
			weapon.isEquipped = true;
		}
	}

	private void CalculateStatFromEquippedWearable (Wearable wearable) {
		if (wearable.bonusses.Count > 0) {
			for (int i = 0; i < wearable.bonusses.Count; i++) {
				wearable.bonusses [i].stat.AddBonus (wearable.bonusses [i].bonus);
				if (wearable.bonusses [i].stat.name == "Vitality") {
					hp.value += wearable.bonusses [i].bonus * 10;
					hp.maxValue = wearable.bonusses [i].stat.GetValue () * 10;
				}
				if (wearable.bonusses [i].stat.name == "Health") {
					hp.value += wearable.bonusses [i].bonus;
					hp.maxValue = wearable.bonusses [i].stat.GetValue ();
				}
			}
		}
	}
	public void CalculateStatFromEquippedWeapon (Weapon weapon) {
		if (weapon.bonusses.Count > 0) {
			for (int i = 0; i < weapon.bonusses.Count; i++) {
				weapon.bonusses [i].stat.AddBonus (weapon.bonusses [i].bonus);
			}
		}
	}


	public void EquipWearable (Wearable wearable, Wearable.WearableSlot slot) {
		if (slot == Wearable.WearableSlot.Kopf) {
			EquipHead (wearable);
		} else if (slot == Wearable.WearableSlot.Körper) {
			EquipBody (wearable);
		} else if (slot == Wearable.WearableSlot.Beine) {
			EquipLegs (wearable);
		} else if (slot == Wearable.WearableSlot.Finger) {
			EquipFinger (wearable);
		}
	}

	public void EquipHead (Wearable itemToEquip) {
		if (headItem)
			UnequipHead ();
		headItem = itemToEquip;
		CalculateStatFromEquippedWearable (headItem);
	}

	private void CalculateStatsFromUnequipWearable (Wearable wearable) {
		if (wearable.bonusses.Count > 0) {
			for (int i = 0; i < wearable.bonusses.Count; i++) {
				wearable.bonusses [i].stat.RemoveBonus (wearable.bonusses [i].bonus);
				if (wearable.bonusses [i].stat.name == "Vitality") {
					hp.value -= wearable.bonusses [i].bonus * 10;
					hp.maxValue = wearable.bonusses [i].stat.GetValue () * 10;
				}
				if (wearable.bonusses [i].stat.name == "Health") {
					hp.value -= wearable.bonusses [i].bonus;
					hp.maxValue = wearable.bonusses [i].stat.GetValue ();
				}
			}
		}
	}

	public void UnequipHead () {
		CalculateStatsFromUnequipWearable (headItem);
		headItem = null;
	}

	public void EquipBody (Wearable itemToEquip) {
		if (bodyItem)
			UnequipBody ();
		bodyItem = itemToEquip;
		CalculateStatFromEquippedWearable (bodyItem);
	}

	public void UnequipBody () {
		CalculateStatsFromUnequipWearable (bodyItem);
		bodyItem = null;
	}

	public void EquipLegs (Wearable itemToEquip) {
		if (legsItem)
			UnequipLegs ();
		legsItem = itemToEquip;
		CalculateStatFromEquippedWearable (legsItem);
	}

	public void UnequipLegs () {
		CalculateStatsFromUnequipWearable (legsItem);
		legsItem = null;
	}


	public void EquipFinger (Wearable itemToEquip) {
		if (fingerItem)
			UnequipFinger ();
		fingerItem = itemToEquip;
		CalculateStatFromEquippedWearable (fingerItem);
	}

	public void UnequipFinger () {
		CalculateStatsFromUnequipWearable (fingerItem);
		fingerItem = null;
	}

	public void EquipWeapon (Weapon itemToEquip) {
		if (weapon)
			UnequipWeapon ();
		weapon = itemToEquip;
		CalculateStatFromEquippedWeapon (weapon);
	}

	public void UnequipWeapon () {
		if (weapon.bonusses.Count > 0) {
			for (int i = 0; i < weapon.bonusses.Count; i++) {
				weapon.bonusses [i].stat.RemoveBonus (weapon.bonusses [i].bonus);
			}
		}
		weapon = null;
	}
}