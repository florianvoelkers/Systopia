﻿using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemEditor : EditorWindow {

	//private Color darkModeBlack = new Color (0.75f, 0.75f, 0.75f);
	//private Color darkModeOrange = new Color (0.9921f, 0.4588f, 0.1294f);
	private GUIStyle labelStyle = new GUIStyle ();
	private string[] itemTypes = { "All Items", "Consumables", "Weapons", "Other" };
	private int selectedItemType = 0;
	private InventoryItem[] itemList;
	private List <bool> collapse = new List<bool> ();

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
	}

	void OnGUI () {

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Item Editor", labelStyle);
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ("Box");
		selectedItemType = GUILayout.SelectionGrid (selectedItemType, itemTypes, 4);
		GUILayout.EndHorizontal ();

		if (GUILayout.Button ("Create Item")) {
			CreateItem ();
		}
		GUILayout.Space (10f);

		if (selectedItemType == 0) {
			itemList = Resources.LoadAll<InventoryItem> ("Items");
			while (collapse.Count < itemList.Length) {
				collapse.Add (true);
			}
			for (int i = 0; i < itemList.Length; i++) {
				GUILayout.BeginHorizontal ();
				collapse[i] = EditorGUILayout.Foldout (collapse[i], itemList[i].itemName);
				if (GUILayout.Button ("Delete Item", GUILayout.MaxWidth (100f))) {
					FileUtil.DeleteFileOrDirectory (Application.dataPath + "/Resources/Items/" + itemList[i].name + ".asset");
					#if UNITY_EDITOR
					UnityEditor.AssetDatabase.Refresh ();
					#endif
				}
				GUILayout.EndHorizontal ();
				if (collapse[i]) {
					GUILayout.BeginHorizontal ();
					GUILayout.Space (30f);
					itemList[i].itemName = EditorGUILayout.TextField ("Item Name", itemList[i].itemName, GUILayout.MaxWidth(450f));
					itemList [i].itemValue = EditorGUILayout.IntField ("Item Value", itemList [i].itemValue, GUILayout.MaxWidth(450f));
					GUILayout.EndHorizontal ();
					GUILayout.BeginHorizontal ();
					GUILayout.Space (30f);
					itemList [i].itemSprite = (Sprite)EditorGUILayout.ObjectField ("Item Sprite", itemList [i].itemSprite, typeof(Sprite), true, GUILayout.MaxWidth(450f), GUILayout.MaxHeight (64f));
					EditorGUILayout.PrefixLabel ("Item Description");
					itemList [i].itemDescription = EditorGUILayout.TextArea (itemList [i].itemDescription, GUILayout.MaxWidth(300f), GUILayout.Height (64f));
					GUILayout.EndHorizontal ();
				}

				GUILayout.Space (5f);
			}
		} else if (selectedItemType == 1) {
			
		} else if (selectedItemType == 2) {
			
		} else if (selectedItemType == 3) {

		}
		Debug.Log (position.width);
	}

	private void CreateItem () {
		Debug.Log ("create item");
	}


}
