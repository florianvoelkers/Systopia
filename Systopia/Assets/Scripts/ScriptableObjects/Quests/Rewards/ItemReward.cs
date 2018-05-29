public class ItemReward : Reward {

	public Item item;
	public PlayerInventory inventory;

	public override void RewardPlayer () {
		inventory.AddItem (item);
	}
}
