using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu (menuName = "Player/Inventory")]
public class PlayerInventory : ScriptableObject {

	public List <Item> items = new List <Item> ();

	public void Reset () {
		items.Clear ();
	}

	public void AddItem (Item item) {
		items.Add (item);
	}

	public void RemoveItem (Item item) {
		items.Remove (item);
	}
}
