using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class ItemEditor : EditorWindow {

	//private Color darkModeBlack = new Color (0.75f, 0.75f, 0.75f);
	//private Color darkModeOrange = new Color (0.9921f, 0.4588f, 0.1294f);
	private GUIStyle labelStyle = new GUIStyle ();
	private string[] itemTypes = { "All Items", "Consumables", "Weapons", "Wearables", "Quest" };
	private int selectedItemType = 0;
	private List <Item> allItems;
	private List <Item> itemList;
	private List <bool> collapse = new List<bool> ();
	private string newItemName = "newItem";

	private PlayerInventory playerInventory;
	private Vector2 scrollPosition;

	[MenuItem ("My Tools/Item Editor")]
	static void Init () {
		EditorWindow window = EditorWindow.GetWindow (typeof(ItemEditor));
		window.position = new Rect(Screen.width / 2, Screen.height / 2, 900, 450);
	}

	void OnLostFocus () {
		for (int i = 0; i < allItems.Count; i++) {
			EditorUtility.SetDirty (allItems [i]);
		}
		AssetDatabase.SaveAssets ();
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
		scrollPosition = EditorGUILayout.BeginScrollView (scrollPosition, false, false);
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Item Editor", labelStyle);
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
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
			} else if (selectedItemType == 4) {
				CreateQuestItem ();
			}
		}
		GUILayout.Space (10f);

		if (selectedItemType == 0) {
			itemList = allItems;
		} else if (selectedItemType == 1) {
			itemList = new List<Item> ();
			for (int i = 0; i < allItems.Count; i++) {
				if (allItems [i] is Consumable) {
					itemList.Add (allItems [i]);
				} 
			}
		} else if (selectedItemType == 2) {
			itemList = new List<Item> ();
			for (int i = 0; i < allItems.Count; i++) {
				if (allItems [i] is Weapon) {
					itemList.Add (allItems [i]);
				} 
			}
		} else if (selectedItemType == 3) {
			itemList = new List<Item> ();
			for (int i = 0; i < allItems.Count; i++) {
				if (allItems [i] is Wearable) {
					itemList.Add (allItems [i]);
				}
			}
		} else if (selectedItemType == 4) {
			itemList = new List <Item> ();
			for (int i = 0; i < allItems.Count; i++) {
				if (allItems [i] is QuestItem) {
					itemList.Add (allItems [i]);
				}
			}
		}

		DisplayItemProperties ();
		EditorGUILayout.EndScrollView ();
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
		Wearable newItemAsWearable = newItem as Wearable;
		if (newItemAsWearable) {
			newItemAsWearable.wearableSlot = Wearable.WearableSlot.Kopf;
		}
		AssetDatabase.CreateAsset (newItem, "Assets/Resources/Items/Wearables/" + newItemName + ".asset");
		AssetDatabase.SaveAssets ();
		EditorUtility.FocusProjectWindow ();
		Selection.activeObject = newItem;
		RefreshItemEditor ();
	}

	private void CreateQuestItem () {
		Item newItem = ScriptableObject.CreateInstance <QuestItem> ();
		newItem.itemName = newItemName;
		AssetDatabase.CreateAsset (newItem, "Assets/Resources/Items/QuestItems/" + newItemName + ".asset");
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
					EditorUtility.SetDirty (playerInventory);
					AssetDatabase.SaveAssets ();
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
					} else if (itemList [i] is QuestItem) {
						assetPath += "QuestItems/";
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
				itemList [i].isStackable = EditorGUILayout.Toggle ("Is Stackable", itemList[i].isStackable, GUILayout.MaxWidth (450f));
				GUILayout.Space (10f);
				itemList [i].itemObject = (GameObject)EditorGUILayout.ObjectField ("Item Object", itemList [i].itemObject, typeof(GameObject), true, GUILayout.MaxWidth(450f));
				GUILayout.EndHorizontal ();

				// check for special values of item subclasses
				Consumable itemAsConsumable = itemList [i] as Consumable;
				if (itemAsConsumable) {
					GUILayout.BeginHorizontal ();
					GUILayout.Space (30f);
					itemAsConsumable.recoveryStat = (IntVariable)EditorGUILayout.ObjectField ("Stat to recover", itemAsConsumable.recoveryStat, typeof (IntVariable), true, GUILayout.MaxWidth (450f));
					GUILayout.Space (10f);
					itemAsConsumable.recoveryValue = EditorGUILayout.IntField ("Recovery Value", itemAsConsumable.recoveryValue, GUILayout.MaxWidth (450f));
					GUILayout.EndHorizontal ();
				}
				Weapon itemAsWeapon = itemList [i] as Weapon;
				if (itemAsWeapon) {
					GUILayout.BeginHorizontal ();
					GUILayout.Space (30f);
					if (GUILayout.Button ("Add stat bonus", GUILayout.Width (150f))) {
						StatBonus newStatBonus = CreateInstance <StatBonus> ();
						newStatBonus.name = "bonus" + itemAsWeapon.bonusses.Count;
						AssetDatabase.AddObjectToAsset (newStatBonus, itemAsWeapon);
						AssetDatabase.ImportAsset (AssetDatabase.GetAssetPath (newStatBonus));
						itemAsWeapon.bonusses.Add (newStatBonus);
						EditorUtility.SetDirty (itemAsWeapon);
						AssetDatabase.SaveAssets ();
						Repaint ();
					}
					GUILayout.EndHorizontal ();
					for (int j = 0; j < itemAsWeapon.bonusses.Count; j++) {
						GUILayout.BeginHorizontal ();
						GUILayout.Space (30f);
						itemAsWeapon.bonusses [j].stat = (Stat)EditorGUILayout.ObjectField ("Stat for bonus", itemAsWeapon.bonusses [j].stat, typeof(Stat), true, GUILayout.MaxWidth (450f));
						GUILayout.Space (10f);
						itemAsWeapon.bonusses [j].bonus = EditorGUILayout.IntField ("Bonus Value", itemAsWeapon.bonusses [j].bonus, GUILayout.MaxWidth (450f));
						GUILayout.Space (10f);
						if (GUILayout.Button ("-", GUILayout.Width (50f))) {
							if (EditorUtility.DisplayDialog ("Delete " + 	itemAsWeapon.bonusses [j].name, "Are you sure you want to delete " + 	itemAsWeapon.bonusses [j].name + "?", "Yes", "No")) {
								StatBonus statBonusToRemove = 	itemAsWeapon.bonusses [j];
								itemAsWeapon.bonusses.RemoveAt (j);
								DestroyImmediate (statBonusToRemove, true);
								AssetDatabase.SaveAssets ();
								EditorUtility.SetDirty (itemAsWeapon);
							}
						}
						GUILayout.EndHorizontal ();
					}
				}
				Wearable itemAsWearable = itemList [i] as Wearable;
				if (itemAsWearable) {
					GUILayout.BeginHorizontal ();
					GUILayout.Space (30f);
					itemAsWearable.wearableSlot = (Wearable.WearableSlot)EditorGUILayout.EnumPopup ("Wearable slot", itemAsWearable.wearableSlot, GUILayout.Width(450f));
					GUILayout.Space (10f);
					if (GUILayout.Button ("Add stat bonus", GUILayout.Width (150f))) {
						StatBonus newStatBonus = CreateInstance <StatBonus> ();
						newStatBonus.name = "bonus" + itemAsWearable.bonusses.Count;
						AssetDatabase.AddObjectToAsset (newStatBonus, itemAsWearable);
						AssetDatabase.ImportAsset (AssetDatabase.GetAssetPath (newStatBonus));
						itemAsWearable.bonusses.Add (newStatBonus);
						EditorUtility.SetDirty (itemAsWearable);
						AssetDatabase.SaveAssets ();
						Repaint ();
					}
					GUILayout.EndHorizontal ();
					for (int j = 0; j < itemAsWearable.bonusses.Count; j++) {
						GUILayout.BeginHorizontal ();
						GUILayout.Space (30f);
						itemAsWearable.bonusses [j].stat = (Stat)EditorGUILayout.ObjectField ("Stat for bonus", itemAsWearable.bonusses [j].stat, typeof(Stat), true, GUILayout.MaxWidth (450f));
						GUILayout.Space (10f);
						itemAsWearable.bonusses [j].bonus = EditorGUILayout.IntField ("Bonus Value", itemAsWearable.bonusses [j].bonus, GUILayout.MaxWidth (450f));
						GUILayout.Space (10f);
						if (GUILayout.Button ("-", GUILayout.Width (50f))) {
							if (EditorUtility.DisplayDialog ("Delete " + 	itemAsWearable.bonusses [j].name, "Are you sure you want to delete " + 	itemAsWearable.bonusses [j].name + "?", "Yes", "No")) {
								StatBonus statBonusToRemove = 	itemAsWearable.bonusses [j];
								itemAsWearable.bonusses.RemoveAt (j);
								DestroyImmediate (statBonusToRemove, true);
								AssetDatabase.SaveAssets ();
								EditorUtility.SetDirty (itemAsWearable);
							}
						}
						GUILayout.EndHorizontal ();
					}
				}
				QuestItem itemAsQuestItem = itemList [i] as QuestItem;
				if (itemAsQuestItem) {
					GUILayout.BeginHorizontal ();
					GUILayout.Space (30f);
					itemAsQuestItem.correspondingCondition = (Condition)EditorGUILayout.ObjectField ("Condition", itemAsQuestItem.correspondingCondition, typeof(Condition), true, GUILayout.MaxWidth (450f));
					GUILayout.EndHorizontal ();
				}

				GUILayout.BeginHorizontal ();
				GUILayout.Space (30f);
				EditorGUILayout.PrefixLabel ("Item Description");
				itemList [i].itemDescription = GUILayout.TextArea (itemList [i].itemDescription, GUILayout.MaxWidth(300f), GUILayout.Height (64f));
				GUILayout.Space (10f);
				itemList [i].itemSprite = (Sprite)EditorGUILayout.ObjectField ("Item Sprite", itemList [i].itemSprite, typeof(Sprite), true, GUILayout.MaxWidth(450f), GUILayout.MaxHeight (64f));			
				GUILayout.EndHorizontal ();

			}

			GUILayout.Space (5f);
		}
	}
		


}
