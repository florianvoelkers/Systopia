using UnityEngine;

[System.Serializable]
[CreateAssetMenu (menuName = "Item")]
public class Item : ScriptableObject {

	public string itemName = "new item";
	public string itemDescription = "describe it here";
	public int itemValue = 0;
	public Sprite itemSprite = null;
	public GameObject itemObject = null;
	public bool isStackable = false;
	public bool isEquipped = false;

	public virtual bool Use () { return true; }

	public virtual void Drop () {
		Debug.Log ("drop it like its hot");
	}
}
