using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class QuestEditor : EditorWindow {

	private GUIStyle labelStyle = new GUIStyle ();
	private Faction[] allFactions;
	private string[] questFactions;
	private int selectedQuestFaction = 0;
	private string newQuestName = "newQuest";
	private List <Quest> allQuests;
	private List <Quest> questList;
	private List <bool> collapse = new List<bool> ();
	private List <List<bool>> statesCollapse = new List <List<bool>> ();

	[MenuItem ("My Tools/Quest Editor")]
	static void Init () {
		EditorWindow.GetWindow (typeof(QuestEditor));
	}

	void OnEnable () {
		labelStyle.padding = new RectOffset (10, 10, 10, 10);
		labelStyle.fontSize = 31;
		labelStyle.fontStyle = FontStyle.Bold;
		labelStyle.normal.textColor = Color.black;
		labelStyle.alignment = TextAnchor.MiddleCenter;
		allFactions = Resources.LoadAll <Faction> ("Factions");
		allQuests = Resources.LoadAll<Quest> ("Quests").ToList();
		questFactions = new string[allFactions.Length + 1];
		questFactions [0] = "All Factions";
		for (int i = 1; i < questFactions.Length; i++) {
			questFactions [i] = allFactions [i-1].factionName;
		}
	}

	void OnGUI () {
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Quest Editor", labelStyle);
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		selectedQuestFaction = GUILayout.SelectionGrid (selectedQuestFaction, questFactions, questFactions.Length);
		GUILayout.EndHorizontal ();

		newQuestName = EditorGUILayout.TextField ("New Quest Name", newQuestName);
		if (GUILayout.Button ("Create Quest")) {
			CreateQuest ();
		}
		GUILayout.Space (10f);

		if (selectedQuestFaction == 0) {
			questList = allQuests;
		} else {
			questList = new List<Quest> ();
			for (int i = 0; i < allQuests.Count; i++) {
				if (allQuests [i].fromFaction.factionName == questFactions [selectedQuestFaction]) {
					questList.Add (allQuests[i]);
				}
			}
		}

		while (collapse.Count < questList.Count) {
			collapse.Add (false);
		}
		while (statesCollapse.Count < questList.Count) {
			statesCollapse.Add (new List<bool> ());
		}
		for (int i = 0; i < questList.Count; i++) {
			GUILayout.BeginHorizontal ();
			collapse [i] = EditorGUILayout.Foldout (collapse [i], questList [i].name);
			if (GUILayout.Button ("Delete Quest", GUILayout.MaxWidth (100f))) {
				if (EditorUtility.DisplayDialog ("Delete " + questList[i].name, "Are you sure you want to delete " + questList[i].name + "?", "Yes", "No")) {
					string assetPath = Application.dataPath + "/Resources/Quests/";
					FileUtil.DeleteFileOrDirectory (assetPath + questList[i].name + ".asset");
					#if UNITY_EDITOR
					UnityEditor.AssetDatabase.Refresh ();
					#endif
					allQuests = Resources.LoadAll<Quest> ("Quests").ToList();
				}
			}
			GUILayout.EndHorizontal ();

			if (collapse [i]) {
				GUILayout.BeginHorizontal ();
				GUILayout.Space (30f);
				questList[i].questTitle = EditorGUILayout.TextField ("Quest Title", questList[i].questTitle, GUILayout.MaxWidth(350f));
				GUILayout.Space (5f);
				questList [i].fromFaction = (Faction)EditorGUILayout.ObjectField ("From Faction", questList [i].fromFaction, typeof(Faction), true, GUILayout.MaxWidth(350f));
				GUILayout.Space (5f);
				questList [i].isQuestFinished = EditorGUILayout.Toggle ("Is Quest Finished", questList[i].isQuestFinished, GUILayout.MaxWidth (150f));
				GUILayout.EndHorizontal ();

				GUILayout.BeginHorizontal ();
				GUILayout.Space (30f);
				EditorGUILayout.PrefixLabel ("Quest Description");
				questList [i].questDescription = EditorGUILayout.TextArea (questList [i].questDescription, GUILayout.Height (64f), GUILayout.MaxWidth (730f));
				GUILayout.EndHorizontal ();

				GUILayout.BeginHorizontal ();
				GUILayout.Space (30f);
				GUILayout.BeginVertical ("box", GUILayout.MaxWidth (435f));
				GUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField ("States", EditorStyles.boldLabel);
				if (GUILayout.Button ("Add new state", GUILayout.Width (150f))) {
					State newState = ScriptableObject.CreateInstance <State> ();
					newState.name = "newState";
					newState.stateName = "New State";
					newState.stateDescription = "describe it here";
					newState.isStateFinished = false;
					AssetDatabase.AddObjectToAsset (newState, questList[i]);

					// import asset so it is recognised as a joined asset
					AssetDatabase.ImportAsset (AssetDatabase.GetAssetPath (newState));
					questList [i].AddState (newState);
					// mark AllConditions as dirty so editor knows to save changes to it when a project save happens
					EditorUtility.SetDirty (questList[i]);
				}
				GUILayout.EndHorizontal ();
				while (statesCollapse[i].Count < questList[i].states.Count) {
					statesCollapse [i].Add (false);
				}
				for (int j = 0; j < questList[i].states.Count; j++) {
					statesCollapse[i][j] = EditorGUILayout.Foldout (statesCollapse[i][j], questList[i].states[j].name);
				}
				GUILayout.EndVertical ();
				GUILayout.Space (5f);
				GUILayout.BeginVertical ("box", GUILayout.MaxWidth (435f));
				GUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField ("Rewards", EditorStyles.boldLabel);
				if (GUILayout.Button ("Add new reward", GUILayout.Width (150f))) {
					PlayerMoney newReward = ScriptableObject.CreateInstance <PlayerMoney> ();
					newReward.name = "newMoney";
					newReward.money = 10;
					AssetDatabase.AddObjectToAsset (newReward, questList[i]);

					// import asset so it is recognised as a joined asset
					AssetDatabase.ImportAsset (AssetDatabase.GetAssetPath (newReward));
					questList [i].AddReward (newReward);
					// mark AllConditions as dirty so editor knows to save changes to it when a project save happens
					EditorUtility.SetDirty (questList[i]);
				}
				GUILayout.EndHorizontal ();
				GUILayout.EndVertical ();
				GUILayout.EndHorizontal ();
			}

			GUILayout.Space (5f);
		}
	}

	private void CreateQuest () {

	}
}
