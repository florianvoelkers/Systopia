using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class ItemEditor : EditorWindow {

	//private Color darkModeBlack = new Color (0.75f, 0.75f, 0.75f);
	//private Color darkModeOrange = new Color (0.9921f, 0.4588f, 0.1294f);
	private GUIStyle labelStyle = new GUIStyle ();
	private string[] itemTypes = { "All Items", "Consumables", "Weapons", "Wearables" };
	private int selectedItemType = 0;
	private List <Item> allItems;
	private List <Item> itemList;
	private List <bool> collapse = new List<bool> ();
	private string newItemName = "newItem";

	private PlayerInventory playerInventory;

	[MenuItem ("My Tools/Item Editor")]
	static void Init () {
		EditorWindow.GetWindow (typeof(ItemEditor));
	}

	void OnEnable () {
		labelStyle.padding = new RectOffset (10, 10, 10, 10);
		labelStyle.fontSize = 31;
		labelStyle.fontStyle = FontStyle.Bold;
		labelStyle.normal.textColor = Color.black;
		labelStyle.alignment = TextAnchor.MiddleCenter;
		allItems = Resources.LoadAll<Item> ("Items").ToList();
		playerInventory = Resources.Load <PlayerInventory> ("Player/PlayerInventory");
	}

	void OnGUI () {

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Item Editor", labelStyle);
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ("Box");
		selectedItemType = GUILayout.SelectionGrid (selectedItemType, itemTypes, itemTypes.Length);
		GUILayout.EndHorizontal ();
		newItemName = EditorGUILayout.TextField ("New Item Name", newItemName);
		if (GUILayout.Button ("Create Item")) {
			if (selectedItemType == 0) {
				Debug.LogWarning ("Select Item Category first");
			} else if (selectedItemType == 1) {
				CreateConsumable ();
			} else if (selectedItemType == 2) {
				CreateWeapon ();
			} else if (selectedItemType == 3) {
				CreateWearable ();
			}
		}
		GUILayout.Space (10f);

		if (selectedItemType == 0) {
			itemList = allItems;
			DisplayItemProperties ();
		} else if (selectedItemType == 1) {
			itemList = new List<Item> ();
			for (int i = 0; i < allItems.Count; i++) {
				if (allItems [i] is Consumable) {
					itemList.Add (allItems [i]);
				} 
			}
			DisplayItemProperties ();
		} else if (selectedItemType == 2) {
			itemList = new List<Item> ();
			for (int i = 0; i < allItems.Count; i++) {
				if (allItems [i] is Weapon) {
					itemList.Add (allItems [i]);
				} 
			}
			DisplayItemProperties ();
		} else if (selectedItemType == 3) {
			itemList = new List<Item> ();
			for (int i = 0; i < allItems.Count; i++) {
				if (allItems [i] is Wearable) {
					itemList.Add (allItems [i]);
				}
			}
			DisplayItemProperties ();
		} 
	}

	private void CreateConsumable () {
		Item newItem = ScriptableObject.CreateInstance <Consumable> ();
		newItem.itemName = newItemName;
		AssetDatabase.CreateAsset (newItem, "Assets/Resources/Items/Consumables/" + newItemName + ".asset");
		AssetDatabase.SaveAssets ();
		EditorUtility.FocusProjectWindow ();
		Selection.activeObject = newItem;
		RefreshItemEditor ();
	}

	private void CreateWeapon () {
		Item newItem = ScriptableObject.CreateInstance <Weapon> ();
		newItem.itemName = newItemName;
		AssetDatabase.CreateAsset (newItem, "Assets/Resources/Items/Weapons/" + newItemName + ".asset");
		AssetDatabase.SaveAssets ();
		EditorUtility.FocusProjectWindow ();
		Selection.activeObject = newItem;
		RefreshItemEditor ();
	}

	private void CreateWearable () {
		Item newItem = ScriptableObject.CreateInstance <Wearable> ();
		newItem.itemName = newItemName;
		AssetDatabase.CreateAsset (newItem, "Assets/Resources/Items/Wearables/" + newItemName + ".asset");
		AssetDatabase.SaveAssets ();
		EditorUtility.FocusProjectWindow ();
		Selection.activeObject = newItem;
		RefreshItemEditor ();
	}

	private void RefreshItemEditor () {
		allItems = Resources.LoadAll<Item> ("Items").ToList();
		newItemName = "newItem";
		Repaint ();
	}

	private void DisplayItemProperties () {
		while (collapse.Count < itemList.Count) {
			collapse.Add (false);
		}
		for (int i = 0; i < itemList.Count; i++) {
			GUILayout.BeginHorizontal ();
			collapse[i] = EditorGUILayout.Foldout (collapse[i], itemList[i].itemName);
			if (GUILayout.Button ("Add to Inventory", GUILayout.MaxWidth (120f))) {
				if (EditorUtility.DisplayDialog ("Add " + itemList[i].name, "Are you sure you want to add " + itemList[i].name + " to the PlayerInventory?", "Yes", "No")) {
					playerInventory.AddItem (itemList[i]);
				}
			}
			if (GUILayout.Button ("Delete Item", GUILayout.MaxWidth (100f))) {
				if (EditorUtility.DisplayDialog ("Delete " + itemList[i].name, "Are you sure you want to delete " + itemList[i].name + "?", "Yes", "No")) {
					string assetPath = Application.dataPath + "/Resources/Items/";
					if (itemList [i] is Consumable) {
						assetPath += "Consumables/";
					} else if (itemList [i] is Weapon) {
						assetPath += "Weapons/";
					} else if (itemList [i] is Wearable) {
						assetPath += "Wearable/";
					}
					FileUtil.DeleteFileOrDirectory (assetPath + itemList[i].name + ".asset");
					#if UNITY_EDITOR
					UnityEditor.AssetDatabase.Refresh ();
					#endif
					allItems = Resources.LoadAll<Item> ("Items").ToList();
				}
			}
			GUILayout.EndHorizontal ();
			if (collapse[i]) {
				GUILayout.BeginHorizontal ();
				GUILayout.Space (30f);
				itemList[i].itemName = EditorGUILayout.TextField ("Item Name", itemList[i].itemName, GUILayout.MaxWidth(450f));
				GUILayout.Space (10f);
				itemList [i].itemValue = EditorGUILayout.IntField ("Item Value", itemList [i].itemValue, GUILayout.MaxWidth(450f));
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				GUILayout.Space (30f);
				// check for special values of item subclasses
				Consumable itemAsConsumable = itemList [i] as Consumable;
				if (itemAsConsumable) {
					itemAsConsumable.recoveryValue = EditorGUILayout.IntField ("Recovery Value", itemAsConsumable.recoveryValue, GUILayout.MaxWidth (250f));
				}
				Weapon itemAsWeapon = itemList [i] as Weapon;
				if (itemAsWeapon) {
					itemAsWeapon.damage = EditorGUILayout.IntField ("Damage", itemAsWeapon.damage, GUILayout.MaxWidth (250f));
				}
				Wearable itemAsWearable = itemList [i] as Wearable;
				if (itemAsWearable) {
					itemAsWearable.armor = EditorGUILayout.IntField ("Armor", itemAsWearable.armor, GUILayout.MaxWidth (250f));
				}
				GUILayout.Space (33f);
				itemList [i].isStackable = EditorGUILayout.Toggle ("Is Stackable", itemList[i].isStackable, GUILayout.MaxWidth (150f));
				GUILayout.Space (23f);
				itemList [i].itemObject = (Rigidbody)EditorGUILayout.ObjectField ("Item Object", itemList [i].itemObject, typeof(Rigidbody), true, GUILayout.MaxWidth(450f));
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				GUILayout.Space (30f);
				EditorGUILayout.PrefixLabel ("Item Description");
				itemList [i].itemDescription = EditorGUILayout.TextArea (itemList [i].itemDescription, GUILayout.MaxWidth(300f), GUILayout.Height (64f));
				GUILayout.Space (10f);
				itemList [i].itemSprite = (Sprite)EditorGUILayout.ObjectField ("Item Sprite", itemList [i].itemSprite, typeof(Sprite), true, GUILayout.MaxWidth(450f), GUILayout.MaxHeight (64f));			
				GUILayout.EndHorizontal ();
			}

			GUILayout.Space (5f);
		}
	}
		


}
