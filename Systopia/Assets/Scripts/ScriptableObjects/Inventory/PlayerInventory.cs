using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu (menuName = "PlayerInventory")]
public class PlayerInventory : ScriptableObject {

	public List <InventoryItem> items;

	public void AddItem (InventoryItem item) {
		items.Add (item);
	}

	public void RemoveItem (InventoryItem item) {
		items.Remove (item);
	}
}
