using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (QuestReaction))]
public class QuestReactionEditor : ReactionEditor {

	private SerializedProperty questProperty;
	private SerializedProperty stateProperty;
	private const string questReactionPropQuestName = "quest";
	private const string questReactionPropStateName = "state";

	private Quest [] allQuests;
	private string[] allQuestTitle;
	private string[] allStateTitle;
	private int selectedQuest;
	private int selectedState;

	protected override void Init () {
		questProperty = serializedObject.FindProperty (questReactionPropQuestName);
		stateProperty = serializedObject.FindProperty (questReactionPropStateName);

		allQuests = Resources.LoadAll<Quest> ("Quests");
		allQuestTitle = new string[allQuests.Length];
		for (int i = 0; i < allQuests.Length; i++) {
			allQuestTitle [i] = allQuests [i].questTitle;
		}
	}

	protected override void DrawReaction () {
		if (questProperty.objectReferenceValue == null) {
			questProperty.objectReferenceValue = allQuests [0];
			selectedQuest = 0;
		} else {
			for (int i = 0; i < allQuests.Length; i++) {
				if (questProperty.objectReferenceValue == allQuests[i]) {
					selectedQuest = i;
				}
			}
		}

		EditorGUILayout.PropertyField (questProperty);
		EditorGUILayout.PropertyField (stateProperty);

		/*
		selectedQuest = EditorGUILayout.Popup (selectedQuest, allQuestTitle);
		questProperty.objectReferenceValue = allQuests [selectedQuest];

		allStateTitle = new string[allQuests [selectedQuest].states.Count];
		for (int i = 0; i < allQuests[selectedQuest].states.Count; i++) {
			allStateTitle [i] = allQuests [selectedQuest].states [i].stateName;
		}

		if (stateProperty.objectReferenceValue == null) {
			stateProperty.objectReferenceValue = allQuests [selectedQuest].states [0];
		} else {
			for (int i = 0; i < allQuests[selectedQuest].states.Count; i++) {
				if (stateProperty.objectReferenceValue == allQuests [selectedQuest].states [i]) {
					selectedState = i;
				} else {
					selectedState = 0;
				}
			}
		}

		selectedState = EditorGUILayout.Popup (selectedState, allStateTitle);
		stateProperty.objectReferenceValue = allQuests [selectedQuest].states [selectedState];*/
	}

	protected override string GetFoldoutLabel () {
		return "Quest Reaction";
	}
}
