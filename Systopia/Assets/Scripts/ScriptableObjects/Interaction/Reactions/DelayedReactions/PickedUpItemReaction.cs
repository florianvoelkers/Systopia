using UnityEngine;

// to add item to inventory
public class PickedUpItemReaction : DelayedReaction {

	public Item item;
	public PlayerInventory inventory;

	protected override void ImmediateReaction () {
		Debug.Log ("react by giving item");
		inventory.AddItem (item);
	}
}
