using UnityEngine;

// to remove items from the inventory
public class LostItemReaction : DelayedReaction {

	public InventoryItem item;
	private Inventory inventory;

	protected override void SpecificInit () {
		inventory = FindObjectOfType<Inventory> ();
	}

	protected override void ImmediateReaction () {
		inventory.RemoveItem (item);
	}
}
