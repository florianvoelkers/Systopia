using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (PlayerStats))]
public class PlayerStatsEditor : Editor {

	private StatsEditor[] statsEditors;
	private PlayerStats playerStats;
	private string newStatName = "newStat";
	private const float buttonWidth = 30f;

	private void OnEnable () {
		playerStats = (PlayerStats)target;
		if (playerStats.stats == null)
			playerStats.stats = new Stat[0];

		if (statsEditors == null) {
			CreateEditors ();
		}
	}

	private void OnDisable () {
		for (int i = 0; i < statsEditors.Length; i++) {
			DestroyImmediate (statsEditors [i]);
		}
		statsEditors = null;
	}

	public override void OnInspectorGUI () {
		if (statsEditors.Length != playerStats.stats.Length) {
			for (int i = 0; i < statsEditors.Length; i++) {
				DestroyImmediate (statsEditors [i]);
			}

			CreateEditors ();
		}

		for (int i = 0; i < statsEditors.Length; i++) {
			EditorGUILayout.BeginHorizontal ();
			statsEditors [i].OnInspectorGUI ();
			if (GUILayout.Button ("-", GUILayout.Width (buttonWidth)))
				RemoveStat (playerStats.stats [i]);
			EditorGUILayout.EndHorizontal ();
		}

		if (playerStats.stats.Length > 0) {
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
		}

		EditorGUILayout.BeginHorizontal ();

		newStatName = EditorGUILayout.TextField (GUIContent.none, newStatName);
		if (GUILayout.Button ("+", GUILayout.Width (buttonWidth))) {
			AddStat (newStatName);
			newStatName = "newStat";
		}

		EditorGUILayout.EndHorizontal ();
	}

	private void CreateEditors () {
		statsEditors = new StatsEditor[playerStats.stats.Length];
		for (int i = 0; i < statsEditors.Length; i++) {
			statsEditors [i] = CreateEditor (playerStats.stats[i]) as StatsEditor;
		}
	}

	private void AddStat (string name) {
		Stat newStat = StatsEditor.CreateStat (name);
		Undo.RecordObject (newStat, "Created new stat");
		AssetDatabase.AddObjectToAsset (newStat, playerStats);
		AssetDatabase.ImportAsset (AssetDatabase.GetAssetPath (newStat));
		ArrayUtility.Add (ref playerStats.stats, newStat);
		EditorUtility.SetDirty (playerStats);
	}

	private void RemoveStat (Stat stat) {
		Undo.RecordObject (playerStats, "Removing stat");
		ArrayUtility.Remove (ref playerStats.stats, stat);
		DestroyImmediate (stat, true);
		AssetDatabase.SaveAssets ();
		EditorUtility.SetDirty (playerStats);
	}
		

}
