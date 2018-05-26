using UnityEngine;

// to add item to inventory
public class PickedUpItemReaction : DelayedReaction {

	public InventoryItem item;
	private PlayerInventory inventory;

	protected override void SpecificInit () {
		inventory = FindObjectOfType<PlayerInventory> ();
	}

	protected override void ImmediateReaction () {
		inventory.AddItem (item);
	}
}
