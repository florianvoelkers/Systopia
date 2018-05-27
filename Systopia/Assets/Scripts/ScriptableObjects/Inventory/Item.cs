using UnityEngine;

[System.Serializable]
[CreateAssetMenu (menuName = "Item")]
public class Item : ScriptableObject {

	public string itemName = "new item";
	public string itemDescription = "describe it here";
	public int itemValue = 0;
	public Sprite itemSprite = null;
	public Rigidbody itemObject = null;
	public bool isQuestItem = false;
	public bool isStackable = false;
}
