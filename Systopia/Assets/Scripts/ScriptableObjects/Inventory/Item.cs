using UnityEngine;

[System.Serializable]
[CreateAssetMenu (menuName = "Item")]
public class Item : ScriptableObject {

	public enum ItemTypes {Consumable, Weapon, Other};

	public string itemName = "new item";
	public string itemDescription = "describe it here";
	public int itemValue = 0;
	public ItemTypes itemType = ItemTypes.Other;
	public Sprite itemSprite = null;
	public Rigidbody itemObject = null;
	public bool isQuestItem = false;
	public bool isStackable = false;

}
