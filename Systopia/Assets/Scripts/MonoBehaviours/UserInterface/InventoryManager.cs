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
	[SerializeField] private Text itemName;
	[SerializeField] private Text itemDescription;
	[SerializeField] private Text itemValues;

	private GameObject[] items;
	private Type[] types = {typeof(Consumable), typeof(Weapon), typeof(Wearable), typeof(QuestItem)};
	private Type currentType;
	private int selectedGroup;
	private List <Item> currentItems;
	private int selectedItem;

	private void Awake () {	 
		items = new GameObject[maximumItemCount];
		selectedGroup = 0;
		groupButtons [selectedGroup].sprite = tabletGroupSelected;
		currentType = types [selectedGroup];
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
		selectedItem = -1;
		itemName.text = "";
		itemDescription.text = "";
		itemValues.text = "";
		FindAllItemsOfType ();
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
		if (index < currentItems.Count) {
			if (selectedItem != -1) {
				items [selectedItem].transform.GetComponent<Image> ().sprite = itemBorder;
				itemName.text = "";
				itemDescription.text = "";
				itemValues.text = "";
			}
			selectedItem = index;
			items [index].transform.GetComponent<Image> ().sprite = itemBorderSelected;
			itemName.text = currentItems [selectedItem].itemName;
			itemDescription.text = currentItems [selectedItem].itemDescription;
			string valueString = "Wert: " + currentItems [selectedItem].itemValue;
			Consumable itemAsConsumable = currentItems [selectedItem] as Consumable;
			if (itemAsConsumable) {
				valueString += "\n" + "Heilwert: " + itemAsConsumable.recoveryValue;
			}

			Weapon itemAsWeapon = currentItems [selectedItem] as Weapon;
			if (itemAsWeapon) {
				//valueString += "\n" + "Schaden: " + itemAsWeapon.damage;
			}

			Wearable itemAsWearable = currentItems [selectedItem] as Wearable;
			if (itemAsWearable) {
				//valueString += "\n" + "Rüstung: " + itemAsWearable.armor;
			}
			itemValues.text = valueString;
		}
	}

}
