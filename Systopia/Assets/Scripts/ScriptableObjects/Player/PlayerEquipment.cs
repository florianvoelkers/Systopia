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

	private void UnequipAll () {
		
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
		if (headItem.bonusses.Count > 0) {
			for (int i = 0; i < headItem.bonusses.Count; i++) {
				headItem.bonusses [i].stat.AddBonus (headItem.bonusses [i].bonus);
			}
		}
	}

	public void UnequipHead () {
		if (headItem.bonusses.Count > 0) {
			for (int i = 0; i < headItem.bonusses.Count; i++) {
				headItem.bonusses [i].stat.RemoveBonus (headItem.bonusses [i].bonus);
			}
		}
		headItem = null;
	}

	public void EquipBody (Wearable itemToEquip) {
		if (bodyItem)
			UnequipBody ();
		bodyItem = itemToEquip;
		if (bodyItem.bonusses.Count > 0) {
			for (int i = 0; i < bodyItem.bonusses.Count; i++) {
				bodyItem.bonusses [i].stat.AddBonus (bodyItem.bonusses [i].bonus);
			}
		}
	}

	public void UnequipBody () {
		if (bodyItem.bonusses.Count > 0) {
			for (int i = 0; i < bodyItem.bonusses.Count; i++) {
				bodyItem.bonusses [i].stat.RemoveBonus (bodyItem.bonusses [i].bonus);
			}
		}
		bodyItem = null;
	}

	public void EquipLegs (Wearable itemToEquip) {
		if (legsItem)
			UnequipLegs ();
		legsItem = itemToEquip;
		if (legsItem.bonusses.Count > 0) {
			for (int i = 0; i < legsItem.bonusses.Count; i++) {
				legsItem.bonusses [i].stat.AddBonus (legsItem.bonusses [i].bonus);
			}
		}
	}

	public void UnequipLegs () {
		if (legsItem.bonusses.Count > 0) {
			for (int i = 0; i < legsItem.bonusses.Count; i++) {
				legsItem.bonusses [i].stat.RemoveBonus (legsItem.bonusses [i].bonus);
			}
		}
		legsItem = null;
	}


	public void EquipFinger (Wearable itemToEquip) {
		if (fingerItem)
			UnequipFinger ();
		fingerItem = itemToEquip;
		if (fingerItem.bonusses.Count > 0) {
			for (int i = 0; i < fingerItem.bonusses.Count; i++) {
				fingerItem.bonusses [i].stat.AddBonus (fingerItem.bonusses [i].bonus);
			}
		}
	}

	public void UnequipFinger () {
		if (fingerItem.bonusses.Count > 0) {
			for (int i = 0; i < fingerItem.bonusses.Count; i++) {
				fingerItem.bonusses [i].stat.RemoveBonus (fingerItem.bonusses [i].bonus);
			}
		}
		fingerItem = null;
	}

	public void EquipWeapon (Weapon itemToEquip) {
		if (weapon)
			UnequipWeapon ();
		weapon = itemToEquip;
		if (weapon.bonusses.Count > 0) {
			for (int i = 0; i < weapon.bonusses.Count; i++) {
				weapon.bonusses [i].stat.AddBonus (weapon.bonusses [i].bonus);
			}
		}
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