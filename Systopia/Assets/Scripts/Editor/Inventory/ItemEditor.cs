using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class ItemEditor : EditorWindow {

	//private Color darkModeBlack = new Color (0.75f, 0.75f, 0.75f);
	//private Color darkModeOrange = new Color (0.9921f, 0.4588f, 0.1294f);
	private GUIStyle labelStyle = new GUIStyle ();
	private string[] itemTypes = { "All Items", "Consumables", "Weapons", "Other" };
	private int selectedItemType = 0;
	private List <InventoryItem> allItems;
	private List<InventoryItem> itemList;
	private List <bool> collapse = new List<bool> ();
	private string newItemName = "newItem";

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
		allItems = Resources.LoadAll<InventoryItem> ("Items").ToList();
	}

	void OnGUI () {

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Item Editor", labelStyle);
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ("Box");
		selectedItemType = GUILayout.SelectionGrid (selectedItemType, itemTypes, 4);
		GUILayout.EndHorizontal ();
		newItemName = EditorGUILayout.TextField ("New Item Name", newItemName);
		if (GUILayout.Button ("Create Item")) {
			CreateItem ();
		}
		GUILayout.Space (10f);

		if (selectedItemType == 0) {
			itemList = allItems;
			DisplayItemProperties ();
		} else if (selectedItemType == 1) {
			itemList = new List<InventoryItem> ();
			for (int i = 0; i < allItems.Count; i++) {
				if (allItems [i].itemType == InventoryItem.ItemTypes.Consumable) {
					itemList.Add (allItems[i]);
				} 
			}
			DisplayItemProperties ();
		} else if (selectedItemType == 2) {
			itemList = new List<InventoryItem> ();
			for (int i = 0; i < allItems.Count; i++) {
				if (allItems [i].itemType == InventoryItem.ItemTypes.Weapon) {
					itemList.Add (allItems[i]);
				} 
			}
			DisplayItemProperties ();
		} else if (selectedItemType == 3) {
			itemList = new List<InventoryItem> ();
			for (int i = 0; i < allItems.Count; i++) {
				if (allItems [i].itemType == InventoryItem.ItemTypes.Other) {
					itemList.Add (allItems[i]);
				}
			}
			DisplayItemProperties ();
		}
	}

	private void CreateItem () {
		InventoryItem newItem = ScriptableObject.CreateInstance <InventoryItem> ();
		if (selectedItemType != 0) {
			newItem.itemType = (InventoryItem.ItemTypes) (selectedItemType - 1);
		}
		AssetDatabase.CreateAsset (newItem, "Assets/Resources/Items/" + newItemName + ".asset");
		AssetDatabase.SaveAssets ();
		EditorUtility.FocusProjectWindow ();
		Selection.activeObject = newItem;
		allItems = Resources.LoadAll<InventoryItem> ("Items").ToList();
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
			if (GUILayout.Button ("Delete Item", GUILayout.MaxWidth (100f))) {
				if (EditorUtility.DisplayDialog ("Delete " + itemList[i].name, "Are you sure you want to delete " + itemList[i].name + "?", "Yes", "No")) {
					FileUtil.DeleteFileOrDirectory (Application.dataPath + "/Resources/Items/" + itemList[i].name + ".asset");
					#if UNITY_EDITOR
					UnityEditor.AssetDatabase.Refresh ();
					#endif
					allItems = Resources.LoadAll<InventoryItem> ("Items").ToList();
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
				itemList [i].itemSprite = (Sprite)EditorGUILayout.ObjectField ("Item Sprite", itemList [i].itemSprite, typeof(Sprite), true, GUILayout.MaxWidth(450f), GUILayout.MaxHeight (64f));
				GUILayout.Space (10f);
				EditorGUILayout.PrefixLabel ("Item Description");
				itemList [i].itemDescription = EditorGUILayout.TextArea (itemList [i].itemDescription, GUILayout.MaxWidth(300f), GUILayout.Height (64f));
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				GUILayout.Space (30f);
				itemList [i].itemObject = (Rigidbody)EditorGUILayout.ObjectField ("Item Object", itemList [i].itemObject, typeof(Rigidbody), true, GUILayout.MaxWidth(450f));
				GUILayout.Space (10f);
				itemList [i].itemType = (InventoryItem.ItemTypes) EditorGUILayout.EnumPopup ("Item Type", itemList[i].itemType, GUILayout.MaxWidth(450f));
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				GUILayout.Space (30f);
				itemList [i].isQuestItem = EditorGUILayout.Toggle ("Is Quest Item", itemList[i].isQuestItem, GUILayout.MaxWidth (450f));
				GUILayout.Space (10f);
				itemList [i].isStackable = EditorGUILayout.Toggle ("Is Stackable", itemList[i].isStackable, GUILayout.MaxWidth (450f));
				GUILayout.EndHorizontal ();
			}

			GUILayout.Space (5f);
		}
	}
		


}
