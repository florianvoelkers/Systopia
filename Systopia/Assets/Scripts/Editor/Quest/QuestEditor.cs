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
	private string newStateName = "newStateName";
	private List <Quest> allQuests;
	private List <Quest> questList;
	private List <bool> collapse = new List<bool> ();
	private List <List<bool>> statesCollapse = new List <List<bool>> ();
	private int selectedRewardType = 0;
	private string[] rewardTypes = {"Item", "Money", "Exp"};

	[MenuItem ("My Tools/Quest Editor")]
	static void Init () {
		EditorWindow window = EditorWindow.GetWindow (typeof(QuestEditor));
		window.position = new Rect(Screen.width / 2, Screen.height / 2, 900, 450);
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
				GUILayout.BeginHorizontal (GUILayout.MaxWidth (915f));
				GUILayout.Space (30f);
				questList[i].questTitle = EditorGUILayout.TextField ("Quest Title", questList[i].questTitle);
				GUILayout.Space (5f);
				questList [i].fromFaction = (Faction)EditorGUILayout.ObjectField ("From Faction", questList [i].fromFaction, typeof(Faction), true);
				GUILayout.EndHorizontal ();

				GUILayout.BeginHorizontal (GUILayout.MaxWidth (915f));
				GUILayout.Space (30f);
				EditorGUILayout.PrefixLabel ("Quest Description");
				questList [i].questDescription = GUILayout.TextArea (questList [i].questDescription, GUILayout.Height (64f));
				GUILayout.EndHorizontal ();

				GUILayout.BeginHorizontal ();
				GUILayout.Space (30f);
				GUILayout.BeginVertical ("box", GUILayout.MaxWidth (400f));
				GUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField ("States", EditorStyles.boldLabel);
				newStateName = EditorGUILayout.TextField ("", newStateName, GUILayout.MaxWidth (200f));
				if (GUILayout.Button ("+", GUILayout.Width (50f))) {
					State newState = ScriptableObject.CreateInstance <State> ();
					newState.name = newStateName;
					newState.stateName = newStateName;
					newState.stateDescription = "describe it here";
					newState.isStateFinished = false;
					AssetDatabase.AddObjectToAsset (newState, questList[i]);

					// import asset so it is recognised as a joined asset
					AssetDatabase.ImportAsset (AssetDatabase.GetAssetPath (newState));
					questList [i].AddState (newState);
					// mark AllConditions as dirty so editor knows to save changes to it when a project save happens
					EditorUtility.SetDirty (questList[i]);
					newStateName = "newStateName";
				}
				GUILayout.EndHorizontal ();
				while (statesCollapse [i].Count < questList [i].states.Count) {
					statesCollapse [i].Add (true);
				}
				for (int j = 0; j < questList[i].states.Count; j++) {
					GUILayout.BeginHorizontal ();
					statesCollapse[i][j] = EditorGUILayout.Foldout (statesCollapse[i][j], questList[i].states[j].name);
					if (GUILayout.Button ("-", GUILayout.Width (50f))) {
						if (EditorUtility.DisplayDialog ("Delete " + questList[i].states[j].name, "Are you sure you want to delete " + questList[i].states[j].name + "?", "Yes", "No")) {
							State stateToRemove = questList[i].states[j];
							questList [i].states.RemoveAt (j);
							DestroyImmediate (stateToRemove, true);
							AssetDatabase.SaveAssets ();
							EditorUtility.SetDirty (questList[i]);
						}
					}
					GUILayout.EndHorizontal ();
					if (statesCollapse [i] [j]) {
						GUILayout.Space (5f);
						GUILayout.BeginHorizontal ();
						GUILayout.Space (15f);
						questList [i].states [j].stateName = EditorGUILayout.TextField ("Name", questList [i].states [j].stateName);
						GUILayout.EndHorizontal ();
						GUILayout.BeginHorizontal ();
						GUILayout.Space (15f);
						EditorGUILayout.PrefixLabel ("Description");
						questList [i].states[j].stateDescription = GUILayout.TextArea (questList [i].states[j].stateDescription, GUILayout.Height (64f));
						GUILayout.EndHorizontal ();
					}
				}
				GUILayout.EndVertical ();
				GUILayout.Space (5f);
				GUILayout.BeginVertical ("box", GUILayout.MaxWidth (400f));
				GUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField ("Rewards", EditorStyles.boldLabel, GUILayout.MaxWidth (100f));
				selectedRewardType = GUILayout.SelectionGrid (selectedRewardType, rewardTypes, rewardTypes.Length);
				if (GUILayout.Button ("+", GUILayout.Width (50f))) {
					Reward newReward;
					if (selectedRewardType == 0) {
						newReward = ScriptableObject.CreateInstance <ItemReward> ();
						newReward.name = "item";
					} else if (selectedRewardType == 1) {
						newReward = ScriptableObject.CreateInstance <MoneyReward> ();
						newReward.name = "money";
					} else {
						newReward = ScriptableObject.CreateInstance <ExperienceReward> ();
						newReward.name = "exp";
					}			
				
					AssetDatabase.AddObjectToAsset (newReward, questList[i]);

					// import asset so it is recognised as a joined asset
					AssetDatabase.ImportAsset (AssetDatabase.GetAssetPath (newReward));
					questList [i].AddReward (newReward);
					// mark AllConditions as dirty so editor knows to save changes to it when a project save happens
					EditorUtility.SetDirty (questList[i]);
				}
				GUILayout.EndHorizontal ();
				for (int j = 0; j < questList [i].rewards.Count; j++) {
					GUILayout.BeginHorizontal ();
					EditorGUILayout.LabelField (questList[i].rewards[j].name, GUILayout.MaxWidth (100f));
					GUILayout.BeginVertical ();
					ItemReward itemReward = questList [i].rewards [j] as ItemReward;
					if (itemReward) {
						itemReward.item = (Item)EditorGUILayout.ObjectField ("Item", itemReward.item, typeof(Item), true);
						itemReward.inventory = (PlayerInventory)EditorGUILayout.ObjectField ("Inventory", itemReward.inventory, typeof(PlayerInventory), true);
					}
					MoneyReward moneyReward = questList [i].rewards [j] as MoneyReward;
					if (moneyReward) {
						moneyReward.moneyRewarded = EditorGUILayout.IntField ("Money", moneyReward.moneyRewarded);
						moneyReward.playerMoney = (PlayerMoney)EditorGUILayout.ObjectField ("PlayerMoney", moneyReward.playerMoney, typeof(PlayerMoney), true);
					}
					ExperienceReward experienceReward = questList [i].rewards [j] as ExperienceReward;
					if (experienceReward) {
						experienceReward.experienceRewarded = EditorGUILayout.IntField ("Exp", experienceReward.experienceRewarded);
						experienceReward.playerExperience = (PlayerExperience)EditorGUILayout.ObjectField ("PlayerExp", experienceReward.playerExperience, typeof(PlayerExperience), true);
					}
					GUILayout.EndVertical ();
					if (GUILayout.Button ("-", GUILayout.Width (50f))) {
						if (EditorUtility.DisplayDialog ("Delete " + questList[i].rewards[j].name, "Are you sure you want to delete " + questList[i].rewards[j].name + "?", "Yes", "No")) {
							Reward rewardToRemove = questList[i].rewards[j];
							questList [i].rewards.RemoveAt (j);
							DestroyImmediate (rewardToRemove, true);
							AssetDatabase.SaveAssets ();
							EditorUtility.SetDirty (questList[i]);
						}
					}
					GUILayout.EndHorizontal ();
				}
				GUILayout.EndVertical ();
				GUILayout.EndHorizontal ();
			}

			GUILayout.Space (5f);
		}
	}

	private void CreateQuest () {
		Quest newQuest = ScriptableObject.CreateInstance <Quest> ();
		newQuest.questTitle = newQuestName;
		if (selectedQuestFaction == 0) {
			newQuest.fromFaction = allFactions [1];
		} else {
			newQuest.fromFaction = allFactions [selectedQuestFaction - 1];
		}
		newQuest.states = new List <State> ();
		newQuest.rewards = new List <Reward> ();
		AssetDatabase.CreateAsset (newQuest, "Assets/Resources/Quests/" + newQuestName + ".asset");
		AssetDatabase.SaveAssets ();
		EditorUtility.FocusProjectWindow ();
		Selection.activeObject = newQuest;
		allQuests = Resources.LoadAll<Quest> ("Quests").ToList();
		newQuestName = "newQuest";
		Repaint ();
	}
}
