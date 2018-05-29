using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ConditionEditor : EditorWindow {

	private GUIStyle labelStyle = new GUIStyle ();
	private string newConditionName = "newCondition";

	private AllConditions allConditions;
	private bool showConditions = true;

	[MenuItem ("My Tools/Condition Editor")]
	static void Init () {
		EditorWindow window = EditorWindow.GetWindow (typeof(ConditionEditor));
		window.position = new Rect(Screen.width / 2, Screen.height / 2, 900, 450);
	}

	void OnEnable () {
		labelStyle.padding = new RectOffset (10, 10, 10, 10);
		labelStyle.fontSize = 31;
		labelStyle.fontStyle = FontStyle.Bold;
		labelStyle.normal.textColor = Color.black;
		labelStyle.alignment = TextAnchor.MiddleCenter;

		allConditions = AllConditions.Instance;
	}

	void OnGUI () {

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Condition Editor", labelStyle);
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		newConditionName = EditorGUILayout.TextField ("New Condition Name", newConditionName);
		if (GUILayout.Button ("Add new condition")) {
			AddNewCondition ();
		}
		GUILayout.EndHorizontal ();
		GUILayout.Space (30f);

		showConditions = EditorGUILayout.Foldout (showConditions, "All Conditions");
		if (showConditions) {
			for (int i = 0; i < allConditions.conditions.Length; i++) {
				GUILayout.BeginHorizontal ();
				GUILayout.Space (30f);
				EditorGUILayout.LabelField (allConditions.conditions[i].name, GUILayout.MaxWidth (200f));
				allConditions.conditions [i].description = EditorGUILayout.TextField ("Description", allConditions.conditions [i].description, GUILayout.MinWidth(200f));
				allConditions.conditions [i].satisfied = EditorGUILayout.Toggle ("Satisfied", allConditions.conditions[i].satisfied, GUILayout.MaxWidth (200f));
				if (GUILayout.Button ("Delete", GUILayout.MaxWidth (100f))) {
					if (EditorUtility.DisplayDialog ("Delete " + allConditions.conditions [i].description, "Are you sure you want to delete " + allConditions.conditions [i].description + "?", "Yes", "No")) {
						Condition conditionToRemove = allConditions.conditions [i];
						ArrayUtility.Remove (ref AllConditions.Instance.conditions, conditionToRemove);
						DestroyImmediate (conditionToRemove, true);
						AssetDatabase.SaveAssets ();
						EditorUtility.SetDirty (AllConditions.Instance);
					}
				}
				GUILayout.EndHorizontal ();
			}
		}
	}

	private void AddNewCondition () {
		Condition newCondition = CreateInstance<Condition> ();
		newCondition.name = newConditionName;
		newCondition.description = "describe it here";
		newCondition.hash = Animator.StringToHash (newCondition.name);

		AssetDatabase.AddObjectToAsset (newCondition, AllConditions.Instance);

		// import asset so it is recognised as a joined asset
		AssetDatabase.ImportAsset (AssetDatabase.GetAssetPath (newCondition));

		// add the condition to the AllConditions list
		ArrayUtility.Add (ref AllConditions.Instance.conditions, newCondition);

		// mark AllConditions as dirty so editor knows to save changes to it when a project save happens
		EditorUtility.SetDirty (AllConditions.Instance);
		newConditionName = "newConditionName";
		Repaint ();
	}

	[MenuItem("Assets/Create/AllConditions")]
	private static void CreateAllConditionsAsset () {
		if (AllConditions.Instance)
			return;

		AllConditions instance = CreateInstance<AllConditions> ();
		AssetDatabase.CreateAsset (instance, "Assets/Resources/Conditions/AllConditions.asset");

		AllConditions.Instance = instance;
		instance.conditions = new Condition[0];;
	}
}
