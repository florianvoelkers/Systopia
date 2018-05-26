using UnityEngine;

// to remove items from the inventory
public class LostItemReaction : DelayedReaction {

	public InventoryItem item;
	private PlayerInventory inventory;

	protected override void SpecificInit () {
		inventory = FindObjectOfType<PlayerInventory> ();
	}

	protected override void ImmediateReaction () {
		inventory.RemoveItem (item);
	}
}
