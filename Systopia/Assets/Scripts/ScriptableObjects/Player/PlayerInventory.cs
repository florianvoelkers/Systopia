using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu (menuName = "Player/Inventory")]
public class PlayerInventory : ScriptableObject {

	public List <Item> items = new List <Item> ();
	public AllConditions allConditions;

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

	public void RemoveItem (Item item) {
		items.Remove (item);
	}
}
