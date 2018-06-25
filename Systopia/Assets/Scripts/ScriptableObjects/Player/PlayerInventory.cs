using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu (menuName = "Player/Inventory")]
public class PlayerInventory : ScriptableObject {

	public List <Item> items = new List <Item> ();
	public AllConditions allConditions;

	public void Load () {
		allConditions = Resources.Load <AllConditions> ("Conditions/AllConditions");
	}

	public void Reset () {
		items.Clear ();
	}

	public void AddItem (Item item) {
		items.Add (item);

		QuestItem questItem = item as QuestItem;
		if (questItem) {
			for (int i = 0; i < allConditions.conditions.Length; i++) {
				if (questItem.correspondingCondition == allConditions.conditions [i]) {
					allConditions.conditions [i].satisfied = true;
				}
			}
		}
	}

	public bool RemoveItem (Item item) {
		for (int i = 0; i < items.Count; i++) {
			if (items [i] == item) {
				items.Remove (item);
				return true;
			}
		}

		return false;
	}
}
