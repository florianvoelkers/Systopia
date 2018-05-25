using UnityEngine;

// to add item to inventory
public class PickedUpItemReaction : MonoBehaviour {

	public InventoryItem item;
	private Inventory inventory;

	protected override void SpecificInit () {
		inventory = FindObjectOfType<Inventory> ();
	}

	protected override void ImmediateReaction () {
		inventory.AddItem (item);
	}
}
