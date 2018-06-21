using UnityEngine;

// to remove items from the inventory
public class LostItemReaction : DelayedReaction {

	public Item item;
	public PlayerInventory inventory;
	public ReactionCollection hasItemReaction;
	public ReactionCollection doesNotHaveItem;

	protected override void SpecificInit () {
		inventory = FindObjectOfType<PlayerInventory> ();
	}

	protected override void ImmediateReaction () {
		bool hadItem = inventory.RemoveItem (item);
		if (hadItem) {
			if (hasItemReaction != null)
				hasItemReaction.React ();
		} else {
			if (doesNotHaveItem != null)
				doesNotHaveItem.React ();
		}
	}
}
