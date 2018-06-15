using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InventoryManager : MonoBehaviour {

	[SerializeField] private PlayerInventory inventory;
	[SerializeField] private Image[] groupButtons;
	[SerializeField] private Sprite tabletGroup;
	[SerializeField] private Sprite tabletGroupSelected;
	[SerializeField] private const int maximumItemCount = 18;
	[SerializeField] private GameObject itemPrefab;
	[SerializeField] private Transform allItems;
	[SerializeField] private Sprite itemBorder;
	[SerializeField] private Sprite itemBorderSelected;
	[SerializeField] private Sprite itemBorderEmpty;
	[SerializeField] private Sprite itemBorderEquipped;
	[SerializeField] private Text itemName;
	[SerializeField] private Text itemDescription;
	[SerializeField] private Button useButton;
	[SerializeField] private Button dropButton;
	[SerializeField] private GameObject itemValues;
	[SerializeField] private GameObject field;
	[SerializeField] private GameObject fieldValue;

	private GameObject[] items;
	private Type[] types = {typeof(Consumable), typeof(Weapon), typeof(Wearable), typeof(QuestItem)};
	private Type currentType;
	private int selectedGroup;
	private List <Item> currentItems;
	private int selectedItem;
	private List <GameObject> itemValueObjects;

	private void Awake () {	 
		items = new GameObject[maximumItemCount];
		selectedGroup = 0;
		selectedItem = -1;
		groupButtons [selectedGroup].sprite = tabletGroupSelected;
		currentType = types [selectedGroup];
		itemValueObjects = new List<GameObject> ();
		FindAllItemsOfType ();
	}

	private void OnEnable () {
		FindAllItemsOfType ();
	}
	
	public void SelectGroup (int selected) {
		groupButtons [selectedGroup].sprite = tabletGroup;
		selectedGroup = selected;
		groupButtons [selectedGroup].sprite = tabletGroupSelected;
		currentType = types [selectedGroup];
		ClearItemDescription ();
		if (selectedGroup == 0) {
			useButton.GetComponentInChildren <Text> ().text = "Use";
		} else if (selectedGroup == 1 || selectedGroup == 2) {
			useButton.GetComponentInChildren <Text> ().text = "Equip";
		}
		FindAllItemsOfType ();
	}

	private void ClearItemDescription () {
		selectedItem = -1;
		itemName.text = "";
		itemDescription.text = "";
		useButton.gameObject.SetActive(false);
		dropButton.gameObject.SetActive (false);
		for (int i = itemValues.transform.childCount - 1; i >= 0; i--) {
			Destroy (itemValues.transform.GetChild (i).gameObject);
		}
		itemValueObjects.Clear ();
	}

	private void FindAllItemsOfType () {
		currentItems = new List<Item> ();
		for (int i = 0; i < inventory.items.Count; i++) {
			if (inventory.items [i].GetType () == currentType) {
				currentItems.Add (inventory.items [i]);
			}
		}
		DisplayAllItems ();
	}

	private void DisplayAllItems () {
		if (items[0] == null) {
			for (int i = 0; i < items.Length; i++) {
				items [i] = Instantiate (itemPrefab, allItems);
				int localIndex = i;
				UnityEngine.Events.UnityAction itemSelection = () => {
					this.SelectItem (localIndex);
				};
				items [i].GetComponent<Button> ().onClick.AddListener (itemSelection);
			}
		}

		for (int i = 0; i < items.Length; i++) {
			if (i < currentItems.Count) { 
				items [i].transform.GetComponent<Image> ().sprite = itemBorder;
				if (currentItems [i].isEquipped)
					items [i].GetComponent <Image> ().sprite = itemBorderEquipped;
				items [i].transform.GetComponentInChildren <Text> ().enabled = true;
				items [i].transform.GetComponentInChildren <Text> ().text = currentItems [i].itemName;
				items [i].transform.GetChild (1).GetComponent <Image> ().enabled = true;
				items [i].transform.GetChild (1).GetComponent <Image> ().sprite = currentItems [i].itemSprite;
			} else {
				items [i].transform.GetComponent<Image> ().sprite = itemBorderEmpty;
				items [i].transform.GetComponentInChildren <Text> ().enabled = false;
				items [i].transform.GetChild (1).GetComponent <Image> ().enabled = false;
			}
		}
	}

	public void SelectItem (int index) {
		for (int i = itemValues.transform.childCount - 1; i >= 0; i--) {
			Destroy (itemValues.transform.GetChild (i).gameObject);
		}
		itemValueObjects.Clear ();
		if (index < currentItems.Count) {
			if (selectedItem != -1) {
				items [selectedItem].transform.GetComponent<Image> ().sprite = itemBorder;
				if (currentItems [selectedItem].isEquipped) 
					items [selectedItem].transform.GetComponent<Image> ().sprite = itemBorderEquipped;
				itemName.text = "";
				itemDescription.text = "";

			}
			selectedItem = index;
			items [index].transform.GetComponent<Image> ().sprite = itemBorderSelected;
			itemName.text = currentItems [selectedItem].itemName;
			itemDescription.text = currentItems [selectedItem].itemDescription;

			itemValueObjects.Add (Instantiate (field, itemValues.transform));
			itemValueObjects [itemValueObjects.Count - 1].GetComponent <Text> ().text = "Goldwert";
			itemValueObjects.Add (Instantiate (fieldValue, itemValues.transform));
			itemValueObjects [itemValueObjects.Count - 1].GetComponent <Text> ().text = currentItems [selectedItem].itemValue.ToString ();

			Consumable itemAsConsumable = currentItems [selectedItem] as Consumable;
			if (itemAsConsumable) {
				itemValueObjects.Add (Instantiate (field, itemValues.transform));
				itemValueObjects [itemValueObjects.Count - 1].GetComponent <Text> ().text = "Regeneration";
				itemValueObjects.Add (Instantiate (fieldValue, itemValues.transform));
				itemValueObjects [itemValueObjects.Count - 1].GetComponent <Text> ().text = itemAsConsumable.recoveryValue.ToString ();
			}

			Weapon itemAsWeapon = currentItems [selectedItem] as Weapon;
			if (itemAsWeapon) {
				for (int i = 0; i < itemAsWeapon.bonusses.Count; i++) {
					itemValueObjects.Add (Instantiate (field, itemValues.transform));
					itemValueObjects [itemValueObjects.Count - 1].GetComponent <Text> ().text = itemAsWeapon.bonusses [i].stat.statName;
					itemValueObjects.Add (Instantiate (fieldValue, itemValues.transform));
					itemValueObjects [itemValueObjects.Count - 1].GetComponent <Text> ().text =  itemAsWeapon.bonusses [i].bonus.ToString ();
				}
			}

			Wearable itemAsWearable = currentItems [selectedItem] as Wearable;
			if (itemAsWearable) {
				itemValueObjects.Add (Instantiate (field, itemValues.transform));
				itemValueObjects [itemValueObjects.Count - 1].GetComponent <Text> ().text = "Position";
				itemValueObjects.Add (Instantiate (fieldValue, itemValues.transform));
				itemValueObjects [itemValueObjects.Count - 1].GetComponent <Text> ().text = itemAsWearable.wearableSlot.ToString ();
				for (int i = 0; i < itemAsWearable.bonusses.Count; i++) {
					itemValueObjects.Add (Instantiate (field, itemValues.transform));
					itemValueObjects [itemValueObjects.Count - 1].GetComponent <Text> ().text = itemAsWearable.bonusses [i].stat.statName;
					itemValueObjects.Add (Instantiate (fieldValue, itemValues.transform));
					itemValueObjects [itemValueObjects.Count - 1].GetComponent <Text> ().text =  itemAsWearable.bonusses [i].bonus.ToString ();
				}
			}
			if (selectedGroup != 3) {
				useButton.gameObject.SetActive (true);
				dropButton.gameObject.SetActive (true);
			}

		}
	}

	public void UseItem () {
		bool successfulUse = currentItems [selectedItem].Use ();
		if (currentItems [selectedItem].GetType () == types [0] && successfulUse) {
			inventory.RemoveItem (currentItems [selectedItem]);
			ClearItemDescription ();
			FindAllItemsOfType ();
		} else if ((currentItems [selectedItem].GetType () == types [1] || currentItems [selectedItem].GetType () == types [2]) && successfulUse) {
			Debug.Log ("equipped true");
			currentItems [selectedItem].isEquipped = true;
			items [selectedItem].GetComponent <Image> ().sprite = itemBorderEquipped;
		}
	}

	public void DropItem () {
		currentItems [selectedItem].Drop ();
	}

}
