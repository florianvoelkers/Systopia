using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

public class FactionEditor : EditorWindow {

	private GUIStyle labelStyle = new GUIStyle ();
	private string newFactionName = "newFaction";

	private List <Faction> factions;
	private List <bool> showFactions = new List<bool> ();

	[MenuItem ("My Tools/Faction Editor")]
	static void Init () {
		EditorWindow window = EditorWindow.GetWindow (typeof(FactionEditor));
		window.position = new Rect(Screen.width / 2, Screen.height / 2, 900, 450);
	}

	void OnEnable () {
		labelStyle.padding = new RectOffset (10, 10, 10, 10);
		labelStyle.fontSize = 31;
		labelStyle.fontStyle = FontStyle.Bold;
		labelStyle.normal.textColor = Color.black;
		labelStyle.alignment = TextAnchor.MiddleCenter;

		factions = Resources.LoadAll <Faction> ("Factions").ToList ();
	}

	void OnGUI () {

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Faction Editor", labelStyle);
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		newFactionName = EditorGUILayout.TextField ("New Faction Name", newFactionName);
		if (GUILayout.Button ("Add new faction")) {
			AddNewFaction ();
		}
		GUILayout.EndHorizontal ();
		GUILayout.Space (30f);

		EditorGUILayout.LabelField ("All Factions");
		for (int i = 0; i < factions.Count; i++) {
			while (showFactions.Count < factions.Count) {
				showFactions.Add (true);
			}
			GUILayout.BeginHorizontal ();
			GUILayout.Space (30f);
			showFactions [i] = EditorGUILayout.Foldout (showFactions[i], factions[i].name);
			if (GUILayout.Button ("Delete", GUILayout.MaxWidth (100f))) {
				if (EditorUtility.DisplayDialog ("Delete " + factions[i].factionName, "Are you sure you want to delete " + factions[i].factionName + "?", "Yes", "No")) {
					FileUtil.DeleteFileOrDirectory (Application.dataPath + "/Resources/Factions/" + factions[i].name + ".asset");
					#if UNITY_EDITOR
					UnityEditor.AssetDatabase.Refresh ();
					#endif
					factions = Resources.LoadAll <Faction> ("Factions").ToList ();
				}
			}
			GUILayout.EndHorizontal ();

			if (showFactions [i]) {
				GUILayout.BeginHorizontal ();
				GUILayout.Space (60f);
				factions[i].factionName = EditorGUILayout.TextField ("Faction Name", factions[i].factionName, GUILayout.MaxWidth(450f));
				GUILayout.EndHorizontal ();

				GUILayout.BeginHorizontal ();
				GUILayout.Space (60f);
				EditorGUILayout.PrefixLabel ("Faction Description");
				factions[i].factionDescription = EditorGUILayout.TextArea (factions[i].factionDescription, GUILayout.MaxWidth(300f), GUILayout.Height (64f));
				GUILayout.Space (10f);
				factions[i].factionIcon = (Sprite)EditorGUILayout.ObjectField ("Faction Icon", factions[i].factionIcon, typeof(Sprite), true, GUILayout.MaxWidth(250f), GUILayout.MaxHeight (64f));
				GUILayout.EndHorizontal ();
			}
		}
	}

	private void AddNewFaction () {
		Faction newFaction = ScriptableObject.CreateInstance <Faction> ();
		newFaction.factionName = newFactionName;

		AssetDatabase.CreateAsset (newFaction, "Assets/Resources/Factions/" + newFactionName + ".asset");
		AssetDatabase.SaveAssets ();
		EditorUtility.FocusProjectWindow ();
		Selection.activeObject = newFaction;
		factions = Resources.LoadAll <Faction> ("Factions").ToList ();
		newFactionName = "newFaction";
	}
}
